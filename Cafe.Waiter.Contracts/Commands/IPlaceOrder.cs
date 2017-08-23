using System.Collections.Generic;
using CQRSTutorial.Core;

namespace Cafe.Waiter.Contracts.Commands
{
    public interface IPlaceOrder : ICommand
    {
        List<OrderedItem> Items { get; set; }
    }
}