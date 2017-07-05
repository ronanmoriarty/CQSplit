using System.Collections.Generic;
using CQRSTutorial.Core;

namespace Cafe.Domain.Events
{
    public class FoodServed : IEvent
    {
        public int Id { get; set; }
        public int TabId { get; set; }

        public List<int> MenuNumbers;
    }
}