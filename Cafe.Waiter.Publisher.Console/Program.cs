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
            var eventDescriptorRepository = new EventDescriptorRepository(readSessionFactory, IsolationLevel.ReadCommitted);
            var messageBusEventPublisher = new MessageBusEventPublisher(new MessageBusFactory(new EnvironmentVariableMessageBusConfiguration()));
            var publisher = new OutboxToMessageQueuePublisher
            (
                eventDescriptorRepository,
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