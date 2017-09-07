using Cafe.Waiter.Query.Service.Consumers;
using Cafe.Waiter.Query.Service.Projectors;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using CQRSTutorial.Infrastructure;

namespace Cafe.Waiter.Query.Service
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
                    .WithServiceAllInterfaces()
                );
        }
    }
}