using System.Reflection;
using CQRSTutorial.Core;
using CQRSTutorial.DAL.Tests.Common;
using NHibernate;
using NUnit.Framework;

namespace CQRSTutorial.DAL.Tests
{
    [TestFixture, Category(TestConstants.Integration)]
    public class EventStoreTests
    {
        private IEvent _retrievedEvent;
        private const int AggregateId = 234;
        private EventStore _repository;
        private ISession _writeSession;

        [SetUp]
        public void SetUp()
        {
            var sqlExecutor = new SqlExecutor();
            sqlExecutor.ExecuteNonQuery($"DELETE FROM dbo.Events WHERE AggregateId = {AggregateId}"); // Do clean-up at start of tests instead of end, so that if a test fails, we can investigate with data still present.
            _writeSession = SessionFactory.WriteInstance.OpenSession();
            _writeSession.BeginTransaction();
            _repository = CreateRepository();
            _repository.UnitOfWork = new NHibernateUnitOfWork(_writeSession);
        }

        [Test]
        public void InsertAndReadTestEvent()
        {
            const string stringPropertyValue = "John";
            const int intPropertyValue = 123;

            var testEvent = new TestEvent
            {
                AggregateId = AggregateId,
                IntProperty = intPropertyValue,
                StringProperty = stringPropertyValue
            };

            InsertAndRead(testEvent);

            Assert.That(_retrievedEvent is TestEvent);
            var retrievedTabOpenedEvent = (TestEvent) _retrievedEvent;
            Assert.That(retrievedTabOpenedEvent.Id, Is.Not.Null);
            Assert.That(retrievedTabOpenedEvent.AggregateId, Is.EqualTo(testEvent.AggregateId));
            Assert.That(retrievedTabOpenedEvent.IntProperty, Is.EqualTo(intPropertyValue));
            Assert.That(retrievedTabOpenedEvent.StringProperty, Is.EqualTo(stringPropertyValue));
        }

        private EventStore CreateRepository()
        {
            return new EventStore(SessionFactory.ReadInstance, new EventMapper(Assembly.GetExecutingAssembly()));
        }

        private void InsertAndRead(IEvent @event)
        {
            _repository.Add(@event);
            _writeSession.Transaction.Commit();
            _retrievedEvent = _repository.Read(@event.Id);
        }
    }
}