using System.Collections.Generic;
using Cafe.Domain;
using Cafe.Waiter.Contracts;
using Cafe.Waiter.Contracts.Commands;

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

        public List<OrderedItem> FoodAwaitingServing => GetPrivateInstanceField<List<OrderedItem>>("_foodAwaitingServing");
        public List<OrderedItem> DrinksAwaitingServing => GetPrivateInstanceField<List<OrderedItem>>("_drinksAwaitingServing");
    }
}