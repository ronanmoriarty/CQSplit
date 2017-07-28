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
        private EventToPublishRepository _eventToPublishRepository;
        private ISession _session;
        private readonly Guid _id = new Guid("8BDD0C3C-2680-4678-BFB9-4D379C2DD208");

        [SetUp]
        public void SetUp()
        {
            var sqlExecutor = new SqlExecutor(WriteModelConnectionStringProviderFactory.Instance);
            sqlExecutor.ExecuteNonQuery($"DELETE FROM dbo.EventsToPublish WHERE Id = '{_id}'"); // do clean-up before test runs instead of after, so that if a test fails, we can investigate data.
            _session = SessionFactory.Instance.OpenSession();
            _session.BeginTransaction();
            _eventToPublishRepository = CreateRepository();
            _eventToPublishRepository.UnitOfWork = new NHibernateUnitOfWork(_session);
        }

        [Test]
        public void InsertAndReadEvent()
        {
            const string stringPropertyValue = "John";
            const int intPropertyValue = 123;

            var testEvent = new TestEvent
            {
                Id = _id,
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

        private EventToPublishRepository CreateRepository()
        {
            return new EventToPublishRepository(SessionFactory.Instance, new EventToPublishMapper(Assembly.GetExecutingAssembly()));
        }

        private void InsertAndRead(IEvent @event)
        {
            _eventToPublishRepository.Add(@event);
            _session.Transaction.Commit();
            _retrievedEvent = _eventToPublishRepository.Read(@event.Id);
        }
    }
}