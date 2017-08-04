using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using CQRSTutorial.Infrastructure;

namespace Cafe.Waiter.Web.DependencyInjection
{
    public class MessageBusInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Classes
                    .FromAssemblyContaining<MessageBusFactory>()
                    .InSameNamespaceAs<MessageBusFactory>()
                    .WithServiceSelf()
                    .WithServiceAllInterfaces()
            );
        }
    }
}