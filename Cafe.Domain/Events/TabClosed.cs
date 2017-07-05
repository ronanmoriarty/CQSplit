using CQRSTutorial.Core;

namespace Cafe.Domain.Events
{
    public class TabClosed : IEvent
    {
        public int Id { get; set; }
        public int TabId { get; set; }

        public decimal AmountPaid;
        public decimal OrderValue;
        public decimal TipValue;
    }
}
