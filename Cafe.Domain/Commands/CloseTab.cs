using CQRSTutorial.Core;

namespace Cafe.Domain.Commands
{
    public class CloseTab : ICommand
    {
        public int Id;
        public decimal AmountPaid;
        public int AggregateId { get; set; }
    }
}
