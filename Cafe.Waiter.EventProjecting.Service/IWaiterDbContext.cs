using System.Linq;

namespace Cafe.Waiter.EventProjecting.Service
{
    public interface IWaiterDbContext
    {
        IQueryable<Queries.DAL.Serialized.Menu> Menus { get; }
    }
}