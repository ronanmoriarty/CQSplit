using System.Data;
using System.Reflection;
using CQRSTutorial.Core;
using CQRSTutorial.DAL.Tests.Common;
using NHibernate;
using NUnit.Framework;

namespace CQRSTutorial.DAL.Tests
{
    [TestFixture, Category(TestConstants.Integration)]
    public class EventStoreTests
        : InsertAndReadTest<EventStore, Event>
    {
        private IEvent _retrievedEvent;
        private const int AggregateId = 234;

        protected override EventStore CreateRepository(ISessionFactory readSessionFactory, IsolationLevel isolationLevel)
        {
            return new EventStore(readSessionFactory, isolationLevel, new EventMapper(Assembly.GetExecutingAssembly()));
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

        private void InsertAndRead(IEvent @event)
        {
            Repository.Add(@event);
            WriteSession.Flush();
            _retrievedEvent = Repository.Read(@event.Id);
        }
    }
}