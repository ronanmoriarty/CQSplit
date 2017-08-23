using Cafe.Waiter.Query.Service.Messaging;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using CQRSTutorial.Infrastructure;

namespace Cafe.Waiter.Query.Service
{
    public class WindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Classes
                    .FromThisAssembly()
                    .InSameNamespaceAs<WaiterQueryService>()
                    .LifestyleTransient(),
                Classes
                    .FromThisAssembly()
                    .InSameNamespaceAs<MessageBusEndpointConfiguration>()
                    .WithServiceSelf()
                    .WithServiceAllInterfaces(),
                Classes
                    .FromAssemblyContaining<EnvironmentVariableMessageBusConfiguration>()
                    .InSameNamespaceAs<EnvironmentVariableMessageBusConfiguration>()
                    .Unless(type => type == typeof(MessageBusEventPublisher))
                    .WithServiceSelf()
                    .WithServiceAllInterfaces()
            );
        }
    }
}