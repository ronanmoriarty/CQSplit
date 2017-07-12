﻿using System.Collections.Generic;
using CQRSTutorial.Core;

namespace Cafe.Domain.Events
{
    public class DrinksServed : IEvent
    {
        public int Id { get; set; }
        public int AggregateId { get; set; }

        public List<int> MenuNumbers;
    }
}
