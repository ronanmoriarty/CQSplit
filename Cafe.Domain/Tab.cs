using System.Collections.Generic;
using Cafe.Domain.Commands;
using Cafe.Domain.Events;
using Cafe.Domain.Exceptions;

namespace Cafe.Domain
{
    public class Tab : ICommandHandler<OpenTab>, ICommandHandler<PlaceOrder>
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

        public IEnumerable<IEvent> Handle(PlaceOrder command)
        {
            throw new TabNotOpen();
        }
    }
}