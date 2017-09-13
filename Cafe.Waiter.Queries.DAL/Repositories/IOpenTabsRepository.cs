using System.Collections.Generic;
using Cafe.Waiter.Queries.DAL.Models;

namespace Cafe.Waiter.Queries.DAL.Repositories
{
    public interface IOpenTabsRepository
    {
        IEnumerable<OpenTab> GetOpenTabs();
        void Insert(OpenTab openTab);
    }
}