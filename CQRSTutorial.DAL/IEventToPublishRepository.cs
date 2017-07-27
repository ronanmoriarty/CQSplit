using System;
using CQRSTutorial.Core;

namespace CQRSTutorial.DAL
{
    public interface IEventToPublishRepository : IHaveUnitOfWork
    {
        void Add(IEvent @event);
        IEvent Read(Guid id);
    }
}