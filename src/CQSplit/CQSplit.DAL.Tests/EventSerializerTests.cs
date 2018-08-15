using System;
using System.Reflection;
using Newtonsoft.Json;
using NUnit.Framework;

namespace CQSplit.DAL.Tests
{
    [TestFixture]
    public class EventSerializerTests
    {
        private EventSerializer _eventSerializer;
        private readonly Guid _id = Guid.NewGuid();
        private readonly Guid _commandId = Guid.NewGuid();
        private readonly Guid _aggregateId = Guid.NewGuid();

        [SetUp]
        public void SetUp()
        {
            _eventSerializer = new EventSerializer(Assembly.GetExecutingAssembly());
        }

        [Test]
        public void Event_gets_converted_to_serializable_event()
        {
            var testEvent = new TestEvent
            {
                Id = _id,
                AggregateId = _aggregateId,
                CommandId = _commandId
            };

            var serializedEvent = _eventSerializer.Serialize(testEvent);

            Assert.That(serializedEvent.Id, Is.EqualTo(_id));
            Assert.That(serializedEvent.AggregateId, Is.EqualTo(_aggregateId));
            Assert.That(serializedEvent.CommandId, Is.EqualTo(_commandId));
            Assert.That(serializedEvent.EventType, Is.EqualTo(typeof(TestEvent).FullName));
            Assert.That(serializedEvent.Data, Is.EqualTo(JsonConvert.SerializeObject(testEvent)));
            Assert.That(serializedEvent.Created, Is.GreaterThan(DateTime.Now.AddSeconds(-1.0)));
        }
    }
}
