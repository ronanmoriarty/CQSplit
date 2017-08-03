using System;
using System.Reflection;
using Cafe.Domain.Events;
using Cafe.Waiter.DAL;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using CQRSTutorial.DAL;
using CQRSTutorial.Infrastructure;
using CQRSTutorial.Publisher;
using log4net;
using NHibernate;

namespace Cafe.Waiter.Publish.Service.Installers
{
    public class PublishServiceInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            const string publishQueuedMessages = "publishQueuedMessages";
            var connectionStringProviderFactory = WriteModelConnectionStringProviderFactory.Instance;
            container.Register(
                Classes
                    .FromAssemblyContaining<PublishService>()
                    .InSameNamespaceAs<PublishService>()
                    .Unless(type => type == typeof(PublishService) || type == typeof(OutboxToMessageQueuePublisher))
                    .WithService
                        .Self()
                        .WithServiceDefaultInterfaces()
                    .LifestyleTransient(),
                Classes
                    .FromAssemblyContaining<EventToPublishRepository>()
                    .InSameNamespaceAs<EventToPublishRepository>()
                    .Unless(type => type == typeof(ConnectionStringProviderFactory) || type == typeof(EventToPublishMapper)) // registered below separately
                    .WithService
                        .Self()
                        .WithServiceAllInterfaces()
                    .LifestyleTransient(),
                Classes
                    .FromAssemblyContaining<MessageBusEventPublisher>()
                    .InSameNamespaceAs<MessageBusEventPublisher>()
                    .WithService
                        .Self()
                        .WithServiceDefaultInterfaces()
                    .LifestyleTransient(),
                Component.For<IConnectionStringProviderFactory>()
                    .Instance(connectionStringProviderFactory),
                Component.For<ISessionFactory>()
                    .UsingFactoryMethod(x => new NHibernateConfiguration(connectionStringProviderFactory).CreateSessionFactory()),
                Component.For<Action>()
                    .Instance(() => container.Resolve<OutboxToMessageQueuePublisher>().PublishQueuedMessages())
                    .Named(publishQueuedMessages),
                Component.For<PublishService>()
                    .DependsOn(Dependency.OnComponent("onNewEventQueuedForPublishing", publishQueuedMessages)),
                Component.For<Assembly>()
                    .Instance(typeof(TabOpened).Assembly)
                    .Named("eventsAssembly"),
                Component.For<EventToPublishMapper>()
                    .DependsOn(Dependency.OnComponent("assemblyToInspectForEvents", "eventsAssembly")),
                Component.For<Func<IUnitOfWork>>()
                    .UsingFactoryMethod(() => CreateUnitOfWork(container))
                    .Named("createUnitOfWork"),
                Component.For<ILog>()
                    .Instance(LogManager.GetLogger(typeof(OutboxToMessageQueuePublisher)))
                    .Named("outboxToMessageQueuePublisherLogger"),
                Component.For<OutboxToMessageQueuePublisher>()
                    .Forward(typeof(IOutboxToMessageQueuePublisher))
                    .DependsOn(Dependency.OnComponent("createUnitOfWork", "createUnitOfWork"))
                    .DependsOn(Dependency.OnComponent("logger", "outboxToMessageQueuePublisherLogger"))
            );
        }

        private Func<IUnitOfWork> CreateUnitOfWork(IWindsorContainer container)
        {
            return () => new NHibernateUnitOfWork(container.Resolve<ISessionFactory>().OpenSession());
        }
    }
}
