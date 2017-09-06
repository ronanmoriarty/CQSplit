using System.Collections.Generic;

namespace Cafe.Waiter.Queries.DAL
{
    public interface IOpenTabsRepository
    {
        IEnumerable<OpenTab> GetOpenTabs();
    }
}