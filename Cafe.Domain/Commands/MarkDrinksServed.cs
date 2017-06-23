using System;
using System.Collections.Generic;

namespace Cafe.Domain.Commands
{
    public class MarkDrinksServed
    {
        public Guid TabId;
        public List<int> MenuNumbers;
    }
}
