using System.Collections.Generic;

namespace Cafe.Domain.Events
{
    public class DrinksServed : ITabEvent
    {
        public int Id { get; set; }
        public int TabId { get; set; }

        public List<int> MenuNumbers;
    }
}
