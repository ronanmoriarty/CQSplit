using System;
using CQRSTutorial.Core;
using NHibernate;

namespace CQRSTutorial.DAL
{
    public interface IEventRepository
    {
        void Add(IEvent @event);
        IEvent Read(Guid id);
        ISession WriteSession { get; set; } //TODO get rid of this - implementation details leaking into interface
    }
}