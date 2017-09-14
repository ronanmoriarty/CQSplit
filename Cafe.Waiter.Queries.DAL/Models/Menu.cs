using System.Collections.Generic;

namespace Cafe.Waiter.Queries.DAL.Models
{
    public class Menu
    {
        public IEnumerable<MenuItem> Items { get; set; }
    }
}