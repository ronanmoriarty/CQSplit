using System.Collections.Generic;
using CQRSTutorial.Core;

namespace Cafe.Waiter.Contracts.Commands
{
    public interface IMarkFoodServedCommand : ICommand
    {
        List<int> MenuNumbers { get; set; }
    }
}