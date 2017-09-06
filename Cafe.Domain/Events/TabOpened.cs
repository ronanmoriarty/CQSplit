using System;
using Cafe.Waiter.Contracts.Events;

namespace Cafe.Domain.Events
{
    public class TabOpened : ITabOpened
    {
        public Guid Id { get; set; }
        public Guid AggregateId { get; set; }
        public Guid CommandId { get; set; }
        public int TableNumber { get; set; }
        public string Waiter { get; set; }
    }
}