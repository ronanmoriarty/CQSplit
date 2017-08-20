using System;
using System.Collections.Generic;
using Cafe.Domain.Commands;
using Cafe.Domain.Events;
using Cafe.Waiter.Contracts;
using CQRSTutorial.Core;

namespace Cafe.Domain
{
    public class TabFactory : ICommandHandler<IOpenTab>
    {
        public IEnumerable<IEvent> Handle(IOpenTab command)
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
            return command is IOpenTab;
        }
    }
}