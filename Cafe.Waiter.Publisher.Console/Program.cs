using System;
using System.Configuration;
using System.Data;
using System.Reflection;
using CQRSTutorial.DAL;
using CQRSTutorial.Infrastructure;
using CQRSTutorial.Publisher;

namespace Cafe.Waiter.Publisher.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["CQRSTutorial"].ConnectionString;
            var readSessionFactory = new NHibernateConfiguration(connectionString).CreateSessionFactory(IsolationLevel.ReadCommitted);
            var eventToPublishMapper = new EventToPublishMapper(Assembly.GetExecutingAssembly());
            var eventToPublishRepository = new EventToPublishRepository(readSessionFactory, IsolationLevel.ReadCommitted, null, eventToPublishMapper); // TODO: we might be able to get rid IPublishConfiguration. Need to do a little more reading around fanout exchanges / topics etc with respect to MassTransit. We're not adding EventsToPublish here anyway, so null is fine in this case.
            var messageBusEventPublisher = new MessageBusEventPublisher(new MessageBusFactory(new EnvironmentVariableMessageBusConfiguration()));
            var publisher = new OutboxToMessageQueuePublisher
            (
                eventToPublishRepository,
                messageBusEventPublisher,
                eventToPublishMapper
            );

            publisher.PublishQueuedMessages();
            System.Console.WriteLine("Press any key to exit");
            System.Console.ReadLine();
            Environment.Exit(0);
        }
    }
}