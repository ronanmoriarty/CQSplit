using System;

namespace CQ.Core
{
    public interface ICommand
    {
        Guid Id { get; set; }
        Guid AggregateId { get; set; }
    }
}