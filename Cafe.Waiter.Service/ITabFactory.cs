using System;
using Cafe.Domain;

namespace Cafe.Waiter.Service
{
    public interface ITabFactory
    {
        Tab Create(Guid commandAggregateId);
    }
}