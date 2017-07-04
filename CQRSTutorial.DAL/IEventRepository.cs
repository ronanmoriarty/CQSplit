using System;
using CQRSTutorial.Core;

namespace CQRSTutorial.DAL
{
    public interface IEventRepository
    {
        void Add(IEvent @event);
        IEvent Read(Guid id);
        object UnitOfWork { get; set; }
    }
}