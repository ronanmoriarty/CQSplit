using System;
using System.Reflection;
using CQRSTutorial.Core;
using CQRSTutorial.DAL.Tests.Common;
using NHibernate;
using NUnit.Framework;

namespace CQRSTutorial.DAL.Tests
{
    [TestFixture, Category(TestConstants.Integration)]
    public class EventToPublishRepositoryTests
    {
        private IEvent _retrievedEvent;
        private IPublishConfiguration _publishConfiguration;
        private const string PublishLocation = "some.rabbitmq.topic.*";
        private EventToPublishRepository _repository;
        private ISession _session;
        private const int Id = -1;

        [SetUp]
        public void SetUp()
        {
            var sqlExecutor = new SqlExecutor();
            sqlExecutor.ExecuteNonQuery($"DELETE FROM dbo.EventsToPublish WHERE Id = {Id}"); // do clean-up before test runs instead of after, so that if a test fails, we can investigate data.
            _publishConfiguration = new TestPublishConfiguration(PublishLocation);
            _session = SessionFactory.Instance.OpenSession();
            _session.BeginTransaction();
            _repository = CreateRepository();
            _repository.UnitOfWork = new NHibernateUnitOfWork(_session);
        }

        [Test]
        public void InsertAndReadEvent()
        {
            const string stringPropertyValue = "John";
            const int intPropertyValue = 123;

            var testEvent = new TestEvent
            {
                Id = Id,
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
                Id = Id,
                IntProperty = intPropertyValue,
                StringProperty = stringPropertyValue
            };

            _repository.Add(testEvent);
            _session.Transaction.Commit();
            var eventToPublish = _repository.Get(testEvent.Id);

            Assert.That(eventToPublish.PublishTo, Is.EqualTo(PublishLocation));
        }

        private EventToPublishRepository CreateRepository()
        {
            return new EventToPublishRepository(SessionFactory.Instance, _publishConfiguration, new EventToPublishMapper(Assembly.GetExecutingAssembly()));
        }

        private void InsertAndRead(IEvent @event)
        {
            _repository.Add(@event);
            _session.Transaction.Commit();
            _retrievedEvent = _repository.Read(@event.Id);
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