using CQRSTutorial.Core;

namespace CQRSTutorial.DAL.Tests
{
    internal class TestEvent : IEvent
    {
        public int Id { get; set; }
        public int AggregateId { get; set; }
        public int IntProperty { get; set; }
        public string StringProperty { get; set; }
    }
}