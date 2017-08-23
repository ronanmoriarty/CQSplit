using System;
using Cafe.Domain;

namespace Cafe.Waiter.Service
{
    public interface ITabFactory
    {
        ITab Create(Guid commandAggregateId);
    }
}