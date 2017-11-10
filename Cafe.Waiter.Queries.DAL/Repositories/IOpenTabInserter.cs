using Cafe.Waiter.Queries.DAL.Models;

namespace Cafe.Waiter.Queries.DAL.Repositories
{
    public interface IOpenTabInserter
    {
        void Insert(OpenTab openTab);
    }
}