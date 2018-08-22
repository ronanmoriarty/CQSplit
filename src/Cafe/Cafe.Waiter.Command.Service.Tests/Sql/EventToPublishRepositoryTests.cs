using System;
using Cafe.DAL.Tests.Common;
using Cafe.Waiter.Command.Service.Sql;
using CQSplit.Core;
using CQSplit.DAL;
using NUnit.Framework;
using ConfigurationRoot = Cafe.DAL.Sql.ConfigurationRoot;

namespace Cafe.Waiter.Command.Service.Tests.Sql
{
    [TestFixture, Category(TestConstants.Integration)]
    public class EventToPublishRepositoryTests
    {
        private IEvent _retrievedEvent;
        private EventToPublishRepository _eventToPublishRepository;
        private SqlExecutor _sqlExecutor;
        private readonly Guid _id = new Guid("8BDD0C3C-2680-4678-BFB9-4D379C2DD208");
        private readonly Guid _id1 = new Guid("75BD91C8-AE33-4EA3-B7BF-8E2140433A62");
        private readonly Guid _id2 = new Guid("DB0CBB04-4773-425F-A6B2-17A939568433");
        private readonly Guid _id3 = new Guid("3A0A042A-D107-4876-B43C-347C0A7C0DAD");
        private EventToPublishSerializer _eventToPublishSerializer;
        private string _connectionString;

        [SetUp]
        public void SetUp()
        {
            _connectionString = ConfigurationRoot.Instance["connectionString"];
            CleanUp();
            _eventToPublishSerializer = new EventToPublishSerializer(typeof(TestEvent).Assembly);
            _eventToPublishRepository = CreateRepository();
            _eventToPublishRepository.UnitOfWork = new EventStoreUnitOfWork(_connectionString);
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

        private void AssertCreated(Guid id)
        {
            var createdDate = _sqlExecutor.ExecuteScalar<DateTime>($"SELECT Created FROM dbo.EventsToPublish WHERE Id = '{id}'");
            var tolerance = new TimeSpan(0,0,5);
            Assert.That(createdDate, Is.EqualTo(DateTime.Now).Within(tolerance));
        }

        private EventToPublishRepository CreateRepository()
        {
            return new EventToPublishRepository(_eventToPublishSerializer);
        }

        private void InsertAndRead(IEvent @event)
        {
            _eventToPublishRepository.Add(@event);
            _eventToPublishRepository.UnitOfWork.Commit();
            var eventToPublish = _eventToPublishRepository.Read(@event.Id);
            _retrievedEvent = _eventToPublishSerializer.Deserialize(eventToPublish);
        }

        [TearDown]
        public void TearDown()
        {
            CleanUp(); // would rather leave these records in database after tests finish for diagnosing problems but these records are causing interference in other integration tests.
        }

        private void CleanUp()
        {
            _sqlExecutor = new SqlExecutor(_connectionString);
            _sqlExecutor.ExecuteNonQuery($"DELETE FROM dbo.EventsToPublish WHERE Id IN ('{_id}','{_id1}','{_id2}','{_id3}')");
            _eventToPublishRepository?.UnitOfWork?.Dispose();
        }
    }
}