using System.Linq;

namespace Cafe.Waiter.Queries.DAL
{
    public interface IWaiterDbContext
    {
        IQueryable<Serialized.Menu> Menus { get; }
        IQueryable<Serialized.OpenTab> OpenTabs { get; }
        IQueryable<Serialized.TabDetails> TabDetails { get; }
        void AddOpenTab(Serialized.OpenTab openTab);
    }
}