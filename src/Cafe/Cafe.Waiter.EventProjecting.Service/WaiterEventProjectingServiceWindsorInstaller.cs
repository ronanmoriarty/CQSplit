using System.Reflection;
using Cafe.Waiter.EventProjecting.Service.Consumers;
using Cafe.Waiter.EventProjecting.Service.DAL;
using Cafe.Waiter.EventProjecting.Service.Projectors;
using Cafe.Waiter.Queries.DAL;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using CQRSTutorial.DAL.Common;
using CQRSTutorial.Messaging;
using CQRSTutorial.Messaging.RabbitMq;
using Microsoft.Extensions.Configuration;

namespace Cafe.Waiter.EventProjecting.Service
{
    public class WaiterEventProjectingServiceWindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            var configurationRoot = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.override.json", optional: true)
                .Build();

            container.Register(
                Classes
                    .FromAssembly(Assembly.GetExecutingAssembly())
                    .InSameNamespaceAs<WaiterEventProjectingServiceWindsorInstaller>()
                    .WithServiceSelf()
                    .WithServiceAllInterfaces(),
                Classes
                    .FromAssembly(Assembly.GetExecutingAssembly())
                    .InSameNamespaceAs<ConsumerFactory>()
                    .WithServiceSelf()
                    .WithServiceAllInterfaces(),
                Classes
                    .FromAssembly(Assembly.GetExecutingAssembly())
                    .InSameNamespaceAs<ITabOpenedProjector>()
                    .WithServiceSelf()
                    .WithServiceAllInterfaces(),
                Classes
                    .FromAssemblyContaining<IMessageBusFactory>()
                    .InSameNamespaceAs<IMessageBusFactory>()
                    .Unless(type => type == typeof(InMemoryMessageBusFactory) || type == typeof(InMemoryReceiveEndpointsConfigurator))
                    .WithServiceSelf()
                    .WithServiceAllInterfaces(),
                Classes
                    .FromAssemblyContaining<RabbitMqMessageBusFactory>()
                    .InSameNamespaceAs<RabbitMqMessageBusFactory>()
                    .Unless(type => type == typeof(NoReceiveEndpointsConfigurator))
                    .WithServiceSelf()
                    .WithServiceAllInterfaces(),
                Component
                    .For<IConnectionStringProvider>()
                    .Instance(ReadModelConnectionStringProvider.Instance),
                Component
                    .For<IOpenTabInserter>()
                    .ImplementedBy<OpenTabInserter>(),
                Component
                    .For<IConfigurationRoot>()
                    .Instance(configurationRoot)
                );
        }
    }
}