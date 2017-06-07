using System.Collections.Generic;
using Cafe.Domain.Commands;
using Cafe.Domain.Events;

namespace Cafe.Domain
{
    public class Tab : CommandHandlerBase<OpenTab>
    {
        public Tab(IEventPublisher eventPublisher)
            : base(eventPublisher)
        {
        }

        protected override IEnumerable<Event> OnCommandReceived(OpenTab command)
        {
            yield return new TabOpened
            {
                Id = command.Id,
                TableNumber = command.TableNumber,
                Waiter = command.Waiter
            };
        }
    }
}