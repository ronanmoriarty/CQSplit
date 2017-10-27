using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cafe.Waiter.Events;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using CQRSTutorial.DAL;
using CQRSTutorial.Messaging;
using CQRSTutorial.Publisher;
using log4net;

namespace Cafe.Waiter.Publish.Service.Installers
{
    public class PublishServiceInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            const string publishQueuedMessages = "publishQueuedMessages";
            var connectionStringProviderFactory = WriteModelConnectionStringProviderFactory.Instance;
            container.Register(
                Component
                    .For<PublishService>()
                    .ImplementedBy<PublishService>()
                    .LifestyleSingleton(),
                Component
                    .For<IOutboxToMessageQueuePublisherConfiguration>()
                    .ImplementedBy<OutboxToMessageQueuePublisherConfiguration>()
                    .LifestyleTransient(),
                Classes
                    .FromAssemblyContaining<EventToPublishRepository>()
                    .InSameNamespaceAs<EventToPublishRepository>()
                    .Unless(type =>
                        type == typeof(ConnectionStringProviderFactory)
                        || type == typeof(EventToPublishMapper)
                        || type == typeof(AppConfigConnectionStringProvider)
                        || type == typeof(ConnectionStringOverride)
                    ) // registered below separately
                    .WithService
                        .Self()
                        .WithServiceAllInterfaces()
                    .LifestyleTransient(),
                Classes
                    .FromAssemblyContaining<MessageBusEventPublisher>()
                    .InSameNamespaceAs<MessageBusEventPublisher>()
                    .WithService
                        .Self()
                        .WithServiceAllInterfaces()
                    .LifestyleTransient(),
                Component.For<IConnectionStringProvider>()
                    .Instance(connectionStringProviderFactory.GetConnectionStringProvider()),
                Component.For<Action>()
                    .Instance(() => container.Resolve<OutboxToMessageQueuePublisher>().PublishQueuedMessages())
                    .Named(publishQueuedMessages),
                Component.For<Assembly>()
                    .Instance(typeof(TabOpened).Assembly)
                    .Named("eventsAssembly"),
                Component.For<EventToPublishMapper>()
                    .DependsOn(Dependency.OnComponent("assemblyToInspectForEvents", "eventsAssembly")),
                Component.For<ILog>()
                    .Instance(LogManager.GetLogger(typeof(OutboxToMessageQueuePublisher)))
                    .Named("outboxToMessageQueuePublisherLogger"),
                Component.For<OutboxToMessageQueuePublisher>()
                    .Forward(typeof(IOutboxToMessageQueuePublisher))
                    .DependsOn(Dependency.OnComponent("logger", "outboxToMessageQueuePublisherLogger")),
                Component.For<IMessageBusEndpointConfiguration>()
                    .ImplementedBy<EmptyMessageBusEndpointConfiguration>(),
                Component.For<IConsumerFactory>()
                    .ImplementedBy<ConsumerFactory>()
            );
        }

        public class EmptyMessageBusEndpointConfiguration : IMessageBusEndpointConfiguration
        {
            public EmptyMessageBusEndpointConfiguration()
            {
                ReceiveEndpoints = Enumerable.Empty<ReceiveEndpointMapping>();
            }

            public IEnumerable<ReceiveEndpointMapping> ReceiveEndpoints { get; }
        }
    }
}
