using System.Collections.Generic;

namespace Cafe.Domain.Commands
{
    public class MarkFoodServed
    {
        public int TabId;
        public List<int> MenuNumbers;
    }
}