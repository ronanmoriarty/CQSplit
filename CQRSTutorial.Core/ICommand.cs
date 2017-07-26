using System;

namespace CQRSTutorial.Core
{
    public interface ICommand
    {
        Guid AggregateId { get; set; }
    }
}