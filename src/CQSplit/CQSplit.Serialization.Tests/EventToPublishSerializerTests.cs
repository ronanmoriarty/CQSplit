using System;
using System.Reflection;
using CQSplit.Serialization.Serialized;
using Newtonsoft.Json;
using NUnit.Framework;

namespace CQSplit.Serialization.Tests
{
    [TestFixture]
    public class EventToPublishSerializerTests
    {
        private EventToPublishSerializer _eventToPublishSerializer;
        private readonly Guid _id = new Guid("6DB1103B-673F-4F1C-B7A4-6A5E697DFBB8");
        private readonly Guid _commandId = new Guid("F0428D89-7DC0-44EB-A0A7-C2029ABD181C");
        private readonly Guid _aggregateId = new Guid("91F6CE8F-7432-4C39-8F65-3D3A7B6F9FBC");
        private string StringPropertyValue = "some string value";

        [SetUp]
        public void SetUp()
        {
            _eventToPublishSerializer = new EventToPublishSerializer(Assembly.GetExecutingAssembly());
        }

        [Test]
        public void Event_gets_converted_to_event_to_publish()
        {
            var testEvent = new TestEvent
            {
                Id = _id,
                AggregateId = _aggregateId,
                CommandId = _commandId,
                StringProperty = StringPropertyValue
            };

            var eventToPublish = _eventToPublishSerializer.Serialize(testEvent);

            Assert.That(eventToPublish.Id, Is.EqualTo(_id));
            Assert.That(eventToPublish.EventType, Is.EqualTo(typeof(TestEvent).FullName));
            Assert.That(eventToPublish.Data, Is.EqualTo(JsonConvert.SerializeObject(testEvent)));
            Assert.That(eventToPublish.Created, Is.GreaterThan(DateTime.Now.AddSeconds(-1.0)));
        }

        [Test]
        public void Event_to_publish_gets_deserialized_using_id_from_serialized_event()
        {
            var otherId = Guid.NewGuid();
            var json = $"{{\"Id\":\"{otherId}\",\"AggregateId\":\"{_aggregateId.ToString()}\",\"CommandId\":\"{_commandId.ToString()}\",\"StringProperty\":\"{StringPropertyValue}\"}}";

            var eventToPublish = new EventToPublish
            {
                Id = _id,
                Data = json,
                EventType = typeof(TestEvent).FullName,
                Created = new DateTime(2018, 1, 1)
            };

            var @event = _eventToPublishSerializer.Deserialize(eventToPublish);

            Assert.That(@event.Id, Is.EqualTo(_id));
            Assert.That(@event.AggregateId, Is.EqualTo(_aggregateId));
            Assert.That(@event.CommandId, Is.EqualTo(_commandId));
            Assert.That(@event.GetType(), Is.EqualTo(typeof(TestEvent)));
            Assert.That(((TestEvent)@event).StringProperty, Is.EqualTo(StringPropertyValue));
        }
    }
}
