using System;
using Cafe.Waiter.Queries.DAL.Models;

namespace Cafe.Waiter.Web.Repositories
{
    public interface ITabDetailsRepository
    {
        TabDetails GetTabDetails(Guid id);
    }
}