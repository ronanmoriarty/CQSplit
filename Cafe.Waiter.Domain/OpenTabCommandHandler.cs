using System;
using System.Collections.Generic;
using Cafe.Waiter.Contracts.Commands;
using Cafe.Waiter.Domain.Events;
using CQRSTutorial.Core;

namespace Cafe.Waiter.Domain
{
    public class OpenTabCommandHandler : ICommandHandler<IOpenTabCommand>
    {
        public IEnumerable<IEvent> Handle(IOpenTabCommand command)
        {
            return new IEvent[]
            {
                new TabOpened
                {
                    Id = Guid.NewGuid(),
                    CommandId = command.Id,
                    AggregateId = command.AggregateId,
                    TableNumber = command.TableNumber,
                    Waiter = command.Waiter
                }
            };
        }

        public bool CanHandle(ICommand command)
        {
            return command is IOpenTabCommand;
        }
    }
}