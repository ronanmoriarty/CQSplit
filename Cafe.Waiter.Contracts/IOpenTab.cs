using CQRSTutorial.Core;

namespace Cafe.Waiter.Contracts
{
    public interface IOpenTab : ICommand
    {
        int TableNumber { get; set; }
        string Waiter { get; set; }
    }
}
