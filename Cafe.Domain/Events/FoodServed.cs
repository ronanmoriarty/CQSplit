using System;
using System.Collections.Generic;
using CQRSTutorial.Core;

namespace Cafe.Domain.Events
{
    public class FoodServed : IEvent
    {
        public Guid Id { get; set; }
        public List<int> MenuNumbers;
    }
}