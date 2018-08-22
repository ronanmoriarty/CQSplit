using System;
using System.Reflection;
using Newtonsoft.Json;
using NUnit.Framework;

namespace CQSplit.Serialization.Tests
{
    [TestFixture]
    public class EventSerializerTests
    {
        private EventSerializer _eventSerializer;
        private readonly Guid _id = new Guid("43395e95-1dfe-4ca1-9f31-5d74e992975f");
        private readonly Guid _commandId = new Guid("3f7043c3-1643-4b79-bb07-17db111ac541");
        private readonly Guid _aggregateId = new Guid("f0850cc6-0141-4d88-b648-16b1734185a1");
        private string StringPropertyValue = "some string value";

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
                CommandId = _commandId,
                StringProperty = StringPropertyValue
            };

            var serializedEvent = _eventSerializer.Serialize(testEvent);

            Assert.That(serializedEvent.Id, Is.EqualTo(_id));
            Assert.That(serializedEvent.AggregateId, Is.EqualTo(_aggregateId));
            Assert.That(serializedEvent.CommandId, Is.EqualTo(_commandId));
            Assert.That(serializedEvent.EventType, Is.EqualTo(typeof(TestEvent).FullName));
            Assert.That(serializedEvent.Data, Is.EqualTo(JsonConvert.SerializeObject(testEvent)));
            Assert.That(serializedEvent.Created, Is.GreaterThan(DateTime.Now.AddSeconds(-1.0)));
        }

        [Test]
        public void Event_gets_deserialized_using_id_from_serialized_event()
        {
            var otherId = Guid.NewGuid();
            var json = $"{{\"Id\":\"{otherId}\",\"AggregateId\":\"{_aggregateId.ToString()}\",\"CommandId\":\"{_commandId.ToString()}\",\"StringProperty\":\"{StringPropertyValue}\"}}";

            var serializedEvent = new Serialized.Event
            {
                Id = _id,
                Data = json,
                EventType = typeof(TestEvent).FullName,
                Created = new DateTime(2018, 1, 1)
            };

            var @event = _eventSerializer.Deserialize(serializedEvent);

            Assert.That(@event.Id, Is.EqualTo(_id));
            Assert.That(@event.AggregateId, Is.EqualTo(_aggregateId));
            Assert.That(@event.CommandId, Is.EqualTo(_commandId));
            Assert.That(@event.GetType(), Is.EqualTo(typeof(TestEvent)));
            Assert.That(((TestEvent)@event).StringProperty, Is.EqualTo(StringPropertyValue));
        }
    }
}
