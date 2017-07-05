using CQRSTutorial.Core;

namespace CQRSTutorial.DAL
{
    public interface IEventRepository
    {
        void Add(IEvent @event);
        IEvent Read(int id);
        object UnitOfWork { get; set; }
    }
}