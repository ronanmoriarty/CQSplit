using CQRSTutorial.Core;

namespace CQRSTutorial.DAL
{
    public interface IEventRepository : IHaveUnitOfWork
    {
        void Add(IEvent @event);
        IEvent Read(int id);
    }
}