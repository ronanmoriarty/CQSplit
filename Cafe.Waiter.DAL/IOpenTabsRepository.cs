using System.Collections.Generic;

namespace Cafe.Waiter.DAL
{
    public interface IOpenTabsRepository
    {
        IEnumerable<OpenTab> GetOpenTabs();
    }
}