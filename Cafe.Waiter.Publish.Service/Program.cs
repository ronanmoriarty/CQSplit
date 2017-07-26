using System;
using System.Threading;
using Cafe.Domain.Events;
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
            var connectionStringProviderFactory = new ConnectionStringProviderFactory("CQRSTutorial.Cafe.Waiter", "CQRSTUTORIAL_CAFE_WAITER_CONNECTIONSTRING_OVERRIDE");
            var sessionFactory = new NHibernateConfiguration(connectionStringProviderFactory).CreateSessionFactory();
            var eventToPublishMapper = new EventToPublishMapper(typeof(TabOpened).Assembly);
            var eventToPublishRepository = new EventToPublishRepository(sessionFactory, new PublishConfiguration(), eventToPublishMapper);
            var messageBusFactory = new MessageBusFactory(new EnvironmentVariableMessageBusConfiguration());
            var messageBusEventPublisher = new MessageBusEventPublisher(messageBusFactory);
            _outboxToMessageQueuePublisher = new OutboxToMessageQueuePublisher
            (
                eventToPublishRepository,
                messageBusEventPublisher,
                eventToPublishMapper,
                () => new NHibernateUnitOfWork(sessionFactory.OpenSession())
            );
            var thread = new Thread(PublishQueuedEvents);
            thread.Start();
        }

        private static void PublishQueuedEvents()
        {
            _outboxToMessageQueuePublisher.PublishQueuedMessages();
        }

        private class PublishConfiguration : IPublishConfiguration
        {
            public string GetPublishLocationFor(Type typeToPublish)
            {
                return "dunno"; // TODO: I think this class might be leaving us very shortly.
            }
        }
    }
}
