using Cafe.Waiter.Events;
using Cafe.Waiter.Queries.DAL.Models;
using Cafe.Waiter.Queries.DAL.Repositories;

namespace Cafe.Waiter.EventProjecting.Service.Projectors
{
    public class TabOpenedProjector : ITabOpenedProjector
    {
        private readonly IOpenTabInserter _openTabInserter;

        public TabOpenedProjector(IOpenTabInserter openTabInserter)
        {
            _openTabInserter = openTabInserter;
        }

        public void Project(TabOpened message)
        {
            _openTabInserter.Insert(new OpenTab
            {
                Id = message.Id,
                TableNumber = message.TableNumber,
                Waiter = message.Waiter,
                Status = TabStatus.Seated
            });
        }
    }
}