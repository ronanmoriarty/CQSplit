namespace Cafe.Domain.Commands
{
    public class CloseTab
    {
        public int Id;
        public int TabId { get; set; }
        public decimal AmountPaid;
    }
}
