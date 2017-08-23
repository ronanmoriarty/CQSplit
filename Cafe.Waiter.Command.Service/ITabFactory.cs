using System;
using Cafe.Domain;

namespace Cafe.Waiter.Command.Service
{
    public interface ITabFactory
    {
        ITab Create(Guid commandAggregateId);
    }
}