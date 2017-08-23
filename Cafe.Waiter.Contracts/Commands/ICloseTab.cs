using CQRSTutorial.Core;

namespace Cafe.Waiter.Contracts.Commands
{
    public interface ICloseTab : ICommand
    {
        decimal AmountPaid { get; set; }
    }
}