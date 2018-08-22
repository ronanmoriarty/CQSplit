using System.Collections.Generic;
using CQSplit;

namespace Cafe.Waiter.Contracts.Commands
{
    public interface IPlaceOrderCommand : ICommand
    {
        List<OrderedItem> Items { get; set; }
    }
}