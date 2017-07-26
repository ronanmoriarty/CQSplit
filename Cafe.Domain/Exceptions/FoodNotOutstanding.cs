using System;
using CQRSTutorial.Core;

namespace Cafe.Domain.Exceptions
{
    public class FoodNotOutstanding : IEvent
    {
        public Guid Id { get; set; }
        public Guid AggregateId { get; set; }
        public Guid CommandId { get; set; }
    }
}