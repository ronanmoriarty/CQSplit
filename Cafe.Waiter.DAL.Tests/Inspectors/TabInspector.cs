using System.Collections.Generic;
using Cafe.Domain;

namespace Cafe.Waiter.DAL.Tests.Inspectors
{
    /// <summary>
    /// Provides access to internal Tab details, for testing purposes.
    /// </summary>
    /// <remarks>Allows Tab details to remain private under normal circumstances.</remarks>
    public class TabInspector : Inspector<Tab>
    {
        public TabInspector(Tab tab)
            : base(tab)
        {
        }

        public bool IsOpened => GetPrivateInstanceField<bool>("_isOpened");

        public List<OrderedItem> FoodAwaitingServing => GetPrivateInstanceField<List<OrderedItem>>("_foodAwaitingServing");
    }
}