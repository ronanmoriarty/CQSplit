using System;

namespace Cafe.Waiter.Queries.DAL.Models
{
    public class TabDetails
    {
        public Guid TabId { get; set; }
        public string Waiter { get; set; }
        public int TableNumber { get; set; }
        public TabStatus Status { get; set; }
    }
}
