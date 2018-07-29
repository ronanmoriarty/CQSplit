using Cafe.Waiter.EventProjecting.Service.DAL;
using Cafe.Waiter.Events;
using Cafe.Waiter.Queries.DAL.Models;
using NLog;

namespace Cafe.Waiter.EventProjecting.Service.Projectors
{
    public class TabOpenedProjector : ITabOpenedProjector
    {
        private readonly IOpenTabInserter _openTabInserter;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public TabOpenedProjector(IOpenTabInserter openTabInserter)
        {
            _openTabInserter = openTabInserter;
        }

        public void Project(TabOpened message)
        {
            _logger.Debug($"Projecting {nameof(TabOpened)} message with Id {message.Id} to new OpenTab...");
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