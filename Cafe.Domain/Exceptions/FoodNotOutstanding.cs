using CQRSTutorial.Core;

namespace Cafe.Domain.Exceptions
{
    public class FoodNotOutstanding : IEvent
    {
        public int Id { get; set; }
        public int AggregateId { get; set; }
    }
}