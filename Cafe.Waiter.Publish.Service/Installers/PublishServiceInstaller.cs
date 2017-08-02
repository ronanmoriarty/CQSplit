using System;
using Cafe.Waiter.DAL;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using CQRSTutorial.DAL;
using CQRSTutorial.Publisher;

namespace Cafe.Waiter.Publish.Service.Installers
{
    public class PublishServiceInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            const string publishQueuedMessages = "publishQueuedMessages";
            container.Register(
                Classes
                    .FromAssemblyContaining<PublishService>()
                    .Where(Component.IsInSameNamespaceAs<PublishService>())
                    .Unless(type => type == typeof(PublishService))
                    .WithService
                        .Self()
                        .WithServiceDefaultInterfaces()
                    .LifestyleTransient(),
                Component.For<IConnectionStringProviderFactory>()
                    .Instance(WriteModelConnectionStringProviderFactory.Instance),
                Component.For<Action>()
                    .Instance(() => container.Resolve<OutboxToMessageQueuePublisher>().PublishQueuedMessages())
                    .Named(publishQueuedMessages),
                Component.For<PublishService>()
                    .DependsOn(Dependency.OnComponent("onNewEventQueuedForPublishing", publishQueuedMessages))
            );
        }
    }
}
