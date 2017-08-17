using System.Collections.Generic;
using CQRSTutorial.Core;

namespace Cafe.Waiter.Contracts
{
    public interface IMarkDrinksServed : ICommand
    {
        List<int> MenuNumbers { get; set; }
    }
}