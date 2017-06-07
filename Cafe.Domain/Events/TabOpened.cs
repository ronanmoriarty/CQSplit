using System;

namespace Cafe.Domain.Events
{
    public class TabOpened : Event
    {
        public Guid Id { get; set; }
        public int TableNumber { get; set; }
        public string Waiter { get; set; }
    }
}