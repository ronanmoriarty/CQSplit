using System.Collections.Generic;

namespace Cafe.Waiter.DAL
{
    public interface IOpenTabsProvider
    {
        IEnumerable<OpenTab> GetOpenTabs();
    }
}