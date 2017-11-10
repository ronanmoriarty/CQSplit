using System.Collections.Generic;
using Cafe.Waiter.Queries.DAL.Models;

namespace Cafe.Waiter.Web.Repositories
{
    public interface IOpenTabsRepository
    {
        IEnumerable<OpenTab> GetOpenTabs();
    }
}