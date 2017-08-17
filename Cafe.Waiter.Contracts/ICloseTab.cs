using CQRSTutorial.Core;

namespace Cafe.Waiter.Contracts
{
    public interface ICloseTab : ICommand
    {
        decimal AmountPaid { get; set; }
    }
}