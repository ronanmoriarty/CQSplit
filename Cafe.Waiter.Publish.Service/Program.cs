using System.Threading;
using Cafe.Domain.Events;
using Cafe.Waiter.DAL;
using CQRSTutorial.DAL;
using CQRSTutorial.Infrastructure;
using CQRSTutorial.Publisher;

namespace Cafe.Waiter.Publish.Service
{
    class Program
    {
        private static OutboxToMessageQueuePublisher _outboxToMessageQueuePublisher;

        static void Main(string[] args)
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
            var thread = new Thread(PublishQueuedEvents);
            thread.Start();
        }

        private static void PublishQueuedEvents()
        {
            _outboxToMessageQueuePublisher.PublishQueuedMessages();
        }
    }
}
