using System;
using System.Linq;
using System.Threading;
using CQRSTutorial.Core;
using CQRSTutorial.DAL.Tests.Common;
using CQRSTutorial.Tests.Common;
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
        private SqlExecutor _sqlExecutor;
        private readonly Guid _id = new Guid("8BDD0C3C-2680-4678-BFB9-4D379C2DD208");
        private readonly Guid _id1 = new Guid("75BD91C8-AE33-4EA3-B7BF-8E2140433A62");
        private readonly Guid _id2 = new Guid("DB0CBB04-4773-425F-A6B2-17A939568433");
        private readonly Guid _id3 = new Guid("3A0A042A-D107-4876-B43C-347C0A7C0DAD");

        [SetUp]
        public void SetUp()
        {
            CleanUp();
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
            AssertCreated(_id);
        }

        [Test]
        public void WhenPublishing_MessagesReadOffQueueInChronologicalOrder()
        {
            var testEvent1 = new TestEvent
            {
                Id = _id1
            };
            _eventToPublishRepository.Add(testEvent1);

            Thread.Sleep(1000); // to ensure they don't get recorded with the same time. Doesn't seem to pick up values smaller than this in database for some reason (even though datetime held in DB to millisecond precision).
            var testEvent2 = new TestEvent
            {
                Id = _id2
            };
            _eventToPublishRepository.Add(testEvent2);

            Thread.Sleep(1000);
            var testEvent3 = new TestEvent
            {
                Id = _id3
            };
            _eventToPublishRepository.Add(testEvent3);

            _session.Transaction.Commit();

            var batch = _eventToPublishRepository.GetEventsAwaitingPublishing(2);
            Assert.That(batch.Count, Is.EqualTo(2));
            Assert.That(batch.First().Id, Is.EqualTo(_id1));
            Assert.That(batch.Last().Id, Is.EqualTo(_id2));
        }

        private void AssertCreated(Guid id)
        {
            var createdDate = _sqlExecutor.ExecuteScalar<DateTime>($"SELECT Created FROM dbo.EventsToPublish WHERE Id = '{id}'");
            var oneSecond = new TimeSpan(0,0,1);
            Assert.That(createdDate, Is.EqualTo(DateTime.Now).Within(oneSecond));
        }

        private EventToPublishRepository CreateRepository()
        {
            return new EventToPublishRepository(SessionFactory.Instance, new EventToPublishMapper(typeof(TestEvent).Assembly));
        }

        private void InsertAndRead(IEvent @event)
        {
            _eventToPublishRepository.Add(@event);
            _session.Transaction.Commit();
            _retrievedEvent = _eventToPublishRepository.Read(@event.Id);
        }

        [TearDown]
        public void TearDown()
        {
            CleanUp(); // would rather leave these records in database after tests finish for diagnosing problems but these records are causing interference in other integration tests.
        }

        private void CleanUp()
        {
            _sqlExecutor = new SqlExecutor(WriteModelConnectionStringProviderFactory.Instance);
            _sqlExecutor.ExecuteNonQuery($"DELETE FROM dbo.EventsToPublish WHERE Id IN ('{_id}','{_id1}','{_id2}','{_id3}')");
            if (_session != null && _session.IsOpen)
            {
                _session?.Close();
            }
        }
    }
}