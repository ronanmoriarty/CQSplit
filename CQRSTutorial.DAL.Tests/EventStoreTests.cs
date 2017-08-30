using System;
using CQRSTutorial.Core;
using CQRSTutorial.DAL.Tests.Common;
using CQRSTutorial.Tests.Common;
using NHibernate;
using NUnit.Framework;

namespace CQRSTutorial.DAL.Tests
{
    [TestFixture, Category(TestConstants.Integration)]
    public class EventStoreTests
    {
        private IEvent _retrievedEvent;
        private readonly Guid _aggregateId = new Guid("0227C779-D2FC-4A26-B549-DA82FB00C87C");
        private readonly Guid _commandId = new Guid("5C81C689-FEAA-4420-9B92-AE6C8D08EF3D");
        private EventStore _repository;
        private ISession _session;

        [SetUp]
        public void SetUp()
        {
            var sqlExecutor = new SqlExecutor(WriteModelConnectionStringProviderFactory.Instance);
            sqlExecutor.ExecuteNonQuery($"DELETE FROM dbo.Events WHERE AggregateId = '{_aggregateId}'"); // Do clean-up at start of tests instead of end, so that if a test fails, we can investigate with data still present.
            _session = SessionFactory.Instance.OpenSession();
            _session.BeginTransaction();
            _repository = CreateRepository();
            _repository.UnitOfWork = new NHibernateUnitOfWork(_session);
        }

        [Test]
        public void InsertAndReadTestEvent()
        {
            const string stringPropertyValue = "John";
            const int intPropertyValue = 123;

            var testEvent = new TestEvent
            {
                AggregateId = _aggregateId,
                CommandId = _commandId,
                IntProperty = intPropertyValue,
                StringProperty = stringPropertyValue
            };

            InsertAndRead(testEvent);

            Assert.That(_retrievedEvent is TestEvent);
            var retrievedTabOpenedEvent = (TestEvent) _retrievedEvent;
            Assert.That(retrievedTabOpenedEvent.Id, Is.Not.Null);
            Assert.That(retrievedTabOpenedEvent.AggregateId, Is.EqualTo(testEvent.AggregateId));
            Assert.That(retrievedTabOpenedEvent.CommandId, Is.EqualTo(testEvent.CommandId));
            Assert.That(retrievedTabOpenedEvent.IntProperty, Is.EqualTo(intPropertyValue));
            Assert.That(retrievedTabOpenedEvent.StringProperty, Is.EqualTo(stringPropertyValue));
        }

        private EventStore CreateRepository()
        {
            return new EventStore(SessionFactory.Instance, new EventMapper(typeof(TestEvent).Assembly));
        }

        private void InsertAndRead(IEvent @event)
        {
            _repository.Add(@event);
            _session.Transaction.Commit();
            _retrievedEvent = _repository.Read(@event.Id);
        }
    }
}