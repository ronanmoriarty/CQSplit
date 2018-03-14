using System;

namespace CQRSTutorial.Core
{
    public interface ICommand
    {
        Guid Id { get; set; }
        Guid AggregateId { get; set; }
    }
}