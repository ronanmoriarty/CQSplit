using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using CQRSTutorial.Infrastructure;

namespace Cafe.Waiter.Service
{
    public class WaiterWindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Classes
                    .FromThisAssembly()
                    .InSameNamespaceAs<WaiterWindsorInstaller>()
                    .WithServiceSelf()
                    .WithServiceAllInterfaces(),
                Classes
                    .FromAssemblyContaining<EnvironmentVariableMessageBusConfiguration>()
                    .InSameNamespaceAs<EnvironmentVariableMessageBusConfiguration>()
                    .WithServiceSelf()
                    .WithServiceAllInterfaces()
                );
        }
    }
}