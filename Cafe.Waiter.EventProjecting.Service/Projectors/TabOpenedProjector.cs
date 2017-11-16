using Cafe.Waiter.EventProjecting.Service.DAL;
using Cafe.Waiter.Events;
using Cafe.Waiter.Queries.DAL.Models;

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
                Id = message.AggregateId,
                TableNumber = message.TableNumber,
                Waiter = message.Waiter,
                Status = TabStatus.Seated
            });
        }
    }
}