using CQSplit;

namespace Cafe.Waiter.Contracts.Commands
{
    public interface IOpenTabCommand : ICommand
    {
        int TableNumber { get; set; }
        string Waiter { get; set; }
    }
}
