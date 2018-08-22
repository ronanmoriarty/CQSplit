using System;

namespace CQSplit
{
    public interface ICommand
    {
        Guid Id { get; set; }
        Guid AggregateId { get; set; }
    }
}