using System;
using System.Collections.Generic;
using Cafe.Domain.Commands;
using Cafe.Domain.Events;
using CQRSTutorial.Core;

namespace Cafe.Domain
{
    public class TabFactory : ICommandHandler<OpenTab>
    {
        public IEnumerable<IEvent> Handle(OpenTab command)
        {
            return new IEvent[]
            {
                new TabOpened
                {
                    Id = Guid.NewGuid(),
                    AggregateId = command.AggregateId,
                    TableNumber = command.TableNumber,
                    Waiter = command.Waiter
                }
            };
        }

        public bool CanHandle(ICommand command)
        {
            return command.GetType() == typeof(OpenTab);
        }
    }
}