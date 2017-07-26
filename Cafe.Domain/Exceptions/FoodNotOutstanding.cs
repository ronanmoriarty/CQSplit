using System;
using CQRSTutorial.Core;

namespace Cafe.Domain.Exceptions
{
    public class FoodNotOutstanding : IEvent
    {
        public int Id { get; set; }
        public Guid AggregateId { get; set; }
    }
}