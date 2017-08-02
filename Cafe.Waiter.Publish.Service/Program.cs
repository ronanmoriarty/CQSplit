using Cafe.Domain.Events;
using Cafe.Waiter.DAL;
using CQRSTutorial.DAL;
using CQRSTutorial.Infrastructure;
using CQRSTutorial.Publisher;
using Topshelf;

namespace Cafe.Waiter.Publish.Service
{
    class Program
    {
        private static OutboxToMessageQueuePublisher _outboxToMessageQueuePublisher;

        static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.Service<PublishService>(publishService =>
                {
                    publishService.ConstructUsing(CreatePublishService);
                    publishService.WhenStarted(tc => tc.Start());
                    publishService.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();

                x.SetDisplayName("CQRSTutorial Event Publishing Service");
                x.SetServiceName("cqrstutorial-event-publishing-service");
                x.SetDescription("Service to publish events queue-tables to message queues");
            });
        }

        private static PublishService CreatePublishService()
        {
            var connectionStringProviderFactory = WriteModelConnectionStringProviderFactory.Instance;
            var sessionFactory = new NHibernateConfiguration(connectionStringProviderFactory).CreateSessionFactory();
            var eventToPublishMapper = new EventToPublishMapper(typeof(TabOpened).Assembly);
            var eventToPublishRepository = new EventToPublishRepository(sessionFactory, eventToPublishMapper);
            var messageBusFactory = new MessageBusFactory(new EnvironmentVariableMessageBusConfiguration());
            var messageBusEventPublisher = new MessageBusEventPublisher(messageBusFactory);
            _outboxToMessageQueuePublisher = new OutboxToMessageQueuePublisher
            (
                eventToPublishRepository,
                messageBusEventPublisher,
                eventToPublishMapper,
                () => new NHibernateUnitOfWork(sessionFactory.OpenSession()),
                new OutboxToMessageQueuePublisherConfiguration()
            );

            return new PublishService(connectionStringProviderFactory, () =>
            {
                _outboxToMessageQueuePublisher.PublishQueuedMessages();
            });
        }
    }
}
