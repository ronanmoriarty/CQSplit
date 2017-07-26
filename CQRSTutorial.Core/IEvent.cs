using System;

namespace CQRSTutorial.Core
{
    public interface IEvent
    {
        Guid Id { get; set; }
        Guid AggregateId { get; set; }
        Guid CommandId { get; set; }
    }
}