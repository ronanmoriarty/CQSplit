using Cafe.Waiter.Queries.DAL.NHibernate;
using Cafe.Waiter.Queries.DAL.Repositories;
using Cafe.Waiter.Web.Controllers;
using Cafe.Waiter.Web.Messaging;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using CQRSTutorial.Messaging;
using MassTransit;
using NHibernate;

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
                Component
                    .For<IMenuConfiguration>()
                    .ImplementedBy<MenuConfiguration>()
                    .LifestyleTransient(),
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
                    .LifestyleSingleton(),
                Component
                    .For<ISessionFactory>()
                    .Instance(ReadModelSessionFactory.Instance)
                    .Named("sessionFactory"),
                Component
                    .For<IOpenTabsRepository>()
                    .ImplementedBy<OpenTabsRepository>()
                    .DependsOn(Dependency.OnComponent("sessionFactory", "sessionFactory")),
                Component
                    .For<ITabDetailsRepository>()
                    .ImplementedBy<TabDetailsRepository>()
                    .DependsOn(Dependency.OnComponent("sessionFactory", "sessionFactory")),
                Component
                    .For<IMenuRepository>()
                    .ImplementedBy<MenuRepository>()
                    .DependsOn(Dependency.OnComponent("sessionFactory", "sessionFactory"))
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