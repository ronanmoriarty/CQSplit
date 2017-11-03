using System.Reflection;
using Cafe.Waiter.EventProjecting.Service.Consumers;
using Cafe.Waiter.EventProjecting.Service.Projectors;
using Cafe.Waiter.Queries.DAL;
using Cafe.Waiter.Queries.DAL.Repositories;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using CQRSTutorial.DAL.Common;
using CQRSTutorial.Messaging;

namespace Cafe.Waiter.EventProjecting.Service
{
    public class WaiterEventProjectingServiceWindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
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
                    .Unless(type => type == typeof(MessageBusEventPublisher))
                    .WithServiceSelf()
                    .WithServiceAllInterfaces(),
                Component
                    .For<IConnectionStringProvider>()
                    .Instance(ReadModelConnectionStringProvider.Instance),
                Component
                    .For<IOpenTabsRepository>()
                    .ImplementedBy<OpenTabsRepository>()
                );
        }
    }
}