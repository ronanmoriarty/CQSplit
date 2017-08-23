using System;
using Cafe.Domain;

namespace Cafe.Waiter.Service
{
    public class TabFactory : ITabFactory
    {
        public Tab Create(Guid commandAggregateId)
        {
            return new Tab
            {
                Id = commandAggregateId
            };
        }
    }
}