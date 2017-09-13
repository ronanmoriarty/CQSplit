using System;
using Cafe.Waiter.Queries.DAL.Models;

namespace Cafe.Waiter.Queries.DAL.Repositories
{
    public interface ITabDetailsRepository
    {
        TabDetails Get(Guid id);
    }
}