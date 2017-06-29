using System;
using CQRSTutorial.Core;
using NHibernate;

namespace CQRSTutorial.DAL
{
    public interface IEventRepository
    {
        void Add(IEvent @event, ISession session);
        IEvent Read(Guid id, ISession session);
    }
}