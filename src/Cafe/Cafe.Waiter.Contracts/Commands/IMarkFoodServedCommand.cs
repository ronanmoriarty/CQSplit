using System.Collections.Generic;
using CQ.Core;

namespace Cafe.Waiter.Contracts.Commands
{
    public interface IMarkFoodServedCommand : ICommand
    {
        List<int> MenuNumbers { get; set; }
    }
}