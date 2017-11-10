using Cafe.Waiter.Queries.DAL.Models;

namespace Cafe.Waiter.EventProjecting.Service.DAL
{
    public interface IOpenTabInserter
    {
        void Insert(OpenTab openTab);
    }
}