using System.Collections.Generic;
using Cafe.Domain;

namespace Cafe.Waiter.DAL.Tests
{
    /// <summary>
    /// Provides access to internal Tab details, for testing purposes.
    /// </summary>
    /// <remarks>Allows Tab details to remain private under normal circumstances.</remarks>
    public class TabInspector : Inspector
    {
        private readonly Tab _tab;

        public TabInspector(Tab tab)
        {
            _tab = tab;
        }

        public bool IsOpened => GetPrivateInstanceField<Tab, bool>("_isOpened", _tab);

        public List<OrderedItem> FoodAwaitingServing => GetPrivateInstanceField<Tab, List<OrderedItem>>("_foodAwaitingServing", _tab);
    }
}