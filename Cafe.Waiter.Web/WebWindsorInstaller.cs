using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Cafe.Waiter.Web
{
    public class WebWindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes
                .FromAssemblyContaining<MessageBus>()
                .InSameNamespaceAs<MessageBus>()
                .WithServiceSelf()
                .WithServiceAllInterfaces()
            );
        }
    }
}