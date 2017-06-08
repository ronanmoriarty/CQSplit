using System;

namespace Cafe.Domain.Commands
{
    public class OpenTab
    {
        public Guid TabId { get; set; }
        public int TableNumber { get; set; }
        public string Waiter { get; set; }
    }
}