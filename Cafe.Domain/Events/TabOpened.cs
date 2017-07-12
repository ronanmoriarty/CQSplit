using CQRSTutorial.Core;

namespace Cafe.Domain.Events
{
    public class TabOpened : IEvent
    {
        public int Id { get; set; }
        public int AggregateId { get; set; }
        public int TableNumber { get; set; }
        public string Waiter { get; set; }
    }
}