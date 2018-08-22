using System;

namespace CQSplit.Core
{
    public interface ICommand
    {
        Guid Id { get; set; }
        Guid AggregateId { get; set; }
    }
}