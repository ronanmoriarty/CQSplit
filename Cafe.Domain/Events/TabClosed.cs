using System;
using CQRSTutorial.Core;

namespace Cafe.Domain.Events
{
    public class TabClosed : IEvent
    {
        public Guid Id { get; set; }
        public decimal AmountPaid;
        public decimal OrderValue;
        public decimal TipValue;
    }
}
