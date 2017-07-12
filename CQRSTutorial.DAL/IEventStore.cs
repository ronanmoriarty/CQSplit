using CQRSTutorial.Core;

namespace CQRSTutorial.DAL
{
    public interface IEventStore : IHaveUnitOfWork
    {
        void Add(IEvent @event);
        IEvent Read(int id);
    }
}