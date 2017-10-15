using System;
using System.Collections.Generic;

namespace Cafe.Waiter.Queries.DAL.Models
{
    public class Menu
    {
        public Guid Id { get; set; }
        public IEnumerable<MenuItem> Items { get; set; }
    }
}