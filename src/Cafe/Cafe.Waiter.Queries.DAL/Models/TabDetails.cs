using System;
using System.Collections.Generic;

namespace Cafe.Waiter.Queries.DAL.Models
{
    public class TabDetails
    {
        public Guid Id { get; set; }
        public string Waiter { get; set; }
        public int TableNumber { get; set; }
        public TabStatus Status { get; set; }
        public IEnumerable<TabItem> Items { get; set; }
    }
}
