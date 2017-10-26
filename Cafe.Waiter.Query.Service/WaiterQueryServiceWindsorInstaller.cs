using Cafe.Waiter.EventProjecting.Service.Consumers;
using Cafe.Waiter.EventProjecting.Service.Projectors;
using Cafe.Waiter.Queries.DAL;
using Cafe.Waiter.Queries.DAL.Repositories;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using CQRSTutorial.DAL;
using CQRSTutorial.Messaging;

namespace Cafe.Waiter.EventProjecting.Service
{
    public class WaiterQueryServiceWindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Classes
                    .FromThisAssembly()
                    .InSameNamespaceAs<WaiterQueryServiceWindsorInstaller>()
                    .WithServiceSelf()
                    .WithServiceAllInterfaces(),
                Classes
                    .FromThisAssembly()
                    .InSameNamespaceAs<ConsumerFactory>()
                    .WithServiceSelf()
                    .WithServiceAllInterfaces(),
                Classes
                    .FromThisAssembly()
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
                    .For<IConnectionStringProviderFactory>()
                    .Instance(ReadModelConnectionStringProviderFactory.Instance),
                Component
                    .For<IOpenTabsRepository>()
                    .ImplementedBy<OpenTabsRepository>()
                );
        }
    }
}