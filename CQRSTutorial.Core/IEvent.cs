using System;

namespace CQRSTutorial.Core
{
    public interface IEvent
    {
        int Id { get; set; }
        Guid AggregateId { get; set; }
    }
}