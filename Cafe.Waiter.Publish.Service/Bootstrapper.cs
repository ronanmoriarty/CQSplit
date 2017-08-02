﻿using Cafe.Domain.Events;
using Cafe.Waiter.DAL;
using Castle.Windsor;
using CQRSTutorial.DAL;
using CQRSTutorial.Infrastructure;
using CQRSTutorial.Publisher;
using log4net;

namespace Cafe.Waiter.Publish.Service
{
    public static class Container
    {
        static Container()
        {
            Instance = new WindsorContainer();
        }

        public static IWindsorContainer Instance { get; }
    }

    public class Bootstrapper
    {
        public static PublishService CreatePublishService()
        {
            var connectionStringProviderFactory = WriteModelConnectionStringProviderFactory.Instance;
            var sessionFactory = new NHibernateConfiguration(connectionStringProviderFactory).CreateSessionFactory();
            var eventToPublishMapper = new EventToPublishMapper(typeof(TabOpened).Assembly);
            var eventToPublishRepository = new EventToPublishRepository(sessionFactory, eventToPublishMapper);
            var messageBusFactory = new MessageBusFactory(new EnvironmentVariableMessageBusConfiguration());
            var messageBusEventPublisher = new MessageBusEventPublisher(messageBusFactory);
            var outboxToMessageQueuePublisher = new OutboxToMessageQueuePublisher
            (
                eventToPublishRepository,
                messageBusEventPublisher,
                eventToPublishMapper,
                () => new NHibernateUnitOfWork(sessionFactory.OpenSession()),
                new OutboxToMessageQueuePublisherConfiguration(),
                LogManager.GetLogger(typeof(Program))
            );

            return new PublishService(connectionStringProviderFactory, () =>
            {
                outboxToMessageQueuePublisher.PublishQueuedMessages();
            });
        }
    }
}