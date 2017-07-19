using System;
using System.Data;
using System.Reflection;
using CQRSTutorial.Core;
using CQRSTutorial.DAL.Tests.Common;
using NHibernate;
using NUnit.Framework;

namespace CQRSTutorial.DAL.Tests
{
    [TestFixture, Category(TestConstants.Integration)]
    public class EventToPublishRepositoryTests
        : InsertAndReadTest<EventToPublishRepository, EventToPublish>
    {
        private IEvent _retrievedEvent;
        private readonly IPublishConfiguration _publishConfiguration;
        private const string PublishLocation = "some.rabbitmq.topic.*";
        private readonly EventToPublishRepository _eventToPublishRepository;
        private readonly SqlExecutor _sqlExecutor;

        public EventToPublishRepositoryTests()
        {
            _eventToPublishRepository = new EventToPublishRepository(SessionFactory.ReadInstance, IsolationLevel.ReadUncommitted, null, new EventToPublishMapper(Assembly.GetExecutingAssembly()));
            _publishConfiguration = new TestPublishConfiguration(PublishLocation);
            _sqlExecutor = new SqlExecutor();
        }

        protected override EventToPublishRepository CreateRepository(ISessionFactory readSessionFactory, IsolationLevel isolationLevel)
        {
            return new EventToPublishRepository(readSessionFactory, isolationLevel, _publishConfiguration, new EventToPublishMapper(Assembly.GetExecutingAssembly()));
        }

        [Test]
        public void InsertAndReadEvent()
        {
            const string stringPropertyValue = "John";
            const int intPropertyValue = 123;

            var testEvent = new TestEvent
            {
                Id = GetNewId(),
                IntProperty = intPropertyValue,
                StringProperty = stringPropertyValue
            };

            InsertAndRead(testEvent);

            Assert.That(_retrievedEvent is TestEvent);
            var retrievedTabOpenedEvent = (TestEvent) _retrievedEvent;
            Assert.That(retrievedTabOpenedEvent.Id, Is.EqualTo(testEvent.Id));
            Assert.That(retrievedTabOpenedEvent.IntProperty, Is.EqualTo(intPropertyValue));
            Assert.That(retrievedTabOpenedEvent.StringProperty, Is.EqualTo(stringPropertyValue));
        }

        [Test]
        public void Event_PublishTo_set_according_to_PublishConfiguration()
        {
            const string stringPropertyValue = "John";
            const int intPropertyValue = 123;

            var testEvent = new TestEvent
            {
                Id = GetNewId(),
                IntProperty = intPropertyValue,
                StringProperty = stringPropertyValue
            };

            Repository.Add(testEvent);
            WriteSession.Flush();
            var eventToPublish = _eventToPublishRepository.Get(testEvent.Id);

            Assert.That(eventToPublish.PublishTo, Is.EqualTo(PublishLocation));
        }

        private int GetNewId()
        {
            var maxValue = _sqlExecutor.ExecuteScalar<int?>("SELECT MAX(Id) FROM dbo.EventsToPublish");
            return maxValue + 1 ?? 1;
        }

        private void InsertAndRead(IEvent @event)
        {
            Repository.Add(@event);
            WriteSession.Flush();
            _retrievedEvent = Repository.Read(@event.Id);
        }
    }

    public class TestPublishConfiguration : IPublishConfiguration
    {
        private readonly string _location;

        public TestPublishConfiguration(string location)
        {
            _location = location;
        }
        public string GetPublishLocationFor(Type typeToPublish)
        {
            return _location;
        }
    }
}