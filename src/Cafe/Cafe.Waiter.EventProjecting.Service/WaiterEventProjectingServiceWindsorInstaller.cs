using System.Reflection;
using Cafe.DAL.Common;
using Cafe.Waiter.EventProjecting.Service.Consumers;
using Cafe.Waiter.EventProjecting.Service.DAL;
using Cafe.Waiter.EventProjecting.Service.Projectors;
using Cafe.Waiter.Queries.DAL;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using CQSplit.Messaging;
using CQSplit.Messaging.RabbitMq;
using Microsoft.Extensions.Configuration;
using ConfigurationRoot = Cafe.DAL.Common.ConfigurationRoot;

namespace Cafe.Waiter.EventProjecting.Service
{
    public class WaiterEventProjectingServiceWindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            var configurationRoot = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
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
                    .For<IOpenTabInserter>()
                    .UsingFactoryMethod(kernel => new OpenTabInserter(ConfigurationRoot.Instance["connectionString"])),
                Component
                    .For<IConfigurationRoot>()
                    .Instance(configurationRoot)
                );
        }
    }
}