using System;
using CQRSTutorial.Core;

namespace Cafe.Domain.Events
{
    public class TabOpened : IEvent
    {
        public Guid Id { get; set; }
        public Guid AggregateId { get; set; }
        public Guid CommandId { get; set; }
        public int TableNumber { get; set; }
        public string Waiter { get; set; }
    }
}