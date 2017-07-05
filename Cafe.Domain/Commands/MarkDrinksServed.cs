using System.Collections.Generic;

namespace Cafe.Domain.Commands
{
    public class MarkDrinksServed
    {
        public int TabId;
        public List<int> MenuNumbers;
    }
}
