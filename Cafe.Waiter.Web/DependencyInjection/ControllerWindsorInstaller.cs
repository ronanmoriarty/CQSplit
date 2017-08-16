using Cafe.Waiter.Web.Controllers;
using Cafe.Waiter.Web.Messaging;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using CQRSTutorial.Infrastructure;
using MassTransit;

namespace Cafe.Waiter.Web.DependencyInjection
{
    public class ControllerWindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            var controllerBasedOnDescriptor = Classes
                .FromAssemblyContaining<TabController>()
                .InSameNamespaceAs<TabController>()
                .WithServiceSelf()
                .WithServiceAllInterfaces();

            container.Register(
                Classes
                    .FromAssemblyContaining<IMessageBusFactory>()
                    .InSameNamespaceAs<IMessageBusFactory>()
                    .WithServiceSelf()
                    .WithServiceAllInterfaces()
                    .LifestyleTransient(),
                Classes
                    .FromThisAssembly()
                    .InSameNamespaceAs<RabbitMqMessageBusEndpointConfiguration>()
                    .WithServiceSelf()
                    .WithServiceAllInterfaces()
                    .LifestyleTransient(),
                SetControllerLifestyle(controllerBasedOnDescriptor),
                Component
                    .For<IBusControl>()
                    .UsingFactoryMethod(GetBusControl)
                    .LifestyleSingleton()
            );
        }

        private BasedOnDescriptor SetControllerLifestyle(BasedOnDescriptor controllerBasedOnDescriptor)
        {
            return ControllerLifestyleConfigurator.Instance.SetLifestyle(controllerBasedOnDescriptor);
        }

        private IBusControl GetBusControl(IKernel kernel)
        {
            var messageBusFactory = kernel.Resolve<RabbitMqMessageBusFactory>();
            return messageBusFactory.Create();
        }
    }
}