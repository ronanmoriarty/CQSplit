using System.Linq;

namespace Cafe.Waiter.EventProjecting.Service
{
    public interface IWaiterDbContext
    {
        IQueryable<Queries.DAL.Serialized.Menu> Menus { get; }
        IQueryable<Queries.DAL.Serialized.OpenTab> OpenTabs { get; }
        IQueryable<Queries.DAL.Serialized.TabDetails> TabDetails { get; }
        void AddOpenTab(Queries.DAL.Serialized.OpenTab openTab);
    }
}