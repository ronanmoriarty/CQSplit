using System;
using Cafe.Domain;

namespace Cafe.Waiter.Service
{
    public class TabFactory : ITabFactory
    {
        public ITab Create(Guid commandAggregateId)
        {
            return new Tab
            {
                Id = commandAggregateId
            };
        }
    }
}