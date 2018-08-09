using CQ.Core;

namespace Cafe.Waiter.Contracts.Commands
{
    public interface ICloseTabCommand : ICommand
    {
        decimal AmountPaid { get; set; }
    }
}