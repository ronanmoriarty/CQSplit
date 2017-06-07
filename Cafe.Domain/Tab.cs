using System.Collections.Generic;
using Cafe.Domain.Commands;
using Cafe.Domain.Events;

namespace Cafe.Domain
{
    public class Tab : ICommandHandler<OpenTab>
    {
        public IEnumerable<IEvent> Handle(OpenTab command)
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