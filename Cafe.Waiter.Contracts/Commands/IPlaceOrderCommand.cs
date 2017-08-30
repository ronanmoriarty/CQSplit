using System.Collections.Generic;
using CQRSTutorial.Core;

namespace Cafe.Waiter.Contracts.Commands
{
    public interface IPlaceOrderCommand : ICommand
    {
        List<OrderedItem> Items { get; set; }
    }
}