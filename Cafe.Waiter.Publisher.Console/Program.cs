using System;
using System.Data;
using Cafe.Publisher;
using CQRSTutorial.DAL;
using CQRSTutorial.Infrastructure;

namespace Cafe.Waiter.Publisher.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var readSessionFactory = NHibernateConfiguration.CreateSessionFactory(IsolationLevel.ReadCommitted);
            var eventDescriptorMapper = new EventDescriptorMapper();
            var eventRepository = new EventRepository(readSessionFactory, IsolationLevel.ReadCommitted, null, eventDescriptorMapper); // TODO: refactor to split EventRepository into smaller parts - it's perfectly ok for publishConfiguration to be null at this stage because we're not adding events here - we're only publishing events, and the EventDescriptors have the publish location in them by this point.
            var messageBusEventPublisher = new MessageBusEventPublisher(new MessageBusFactory(new EnvironmentVariableMessageBusConfiguration()));
            var publisher = new OutboxToMessageQueuePublisher
            (
                eventRepository,
                messageBusEventPublisher,
                eventDescriptorMapper
            );

            publisher.PublishQueuedMessages();
            System.Console.WriteLine("Press any key to exit");
            System.Console.ReadLine();
            Environment.Exit(0);
        }
    }
}