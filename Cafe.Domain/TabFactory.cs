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
            Console.WriteLine("Handling OpenTab command...");
            return new IEvent[]
            {
                new TabOpened
                {
                    AggregateId = command.TabId,
                    TableNumber = command.TableNumber,
                    Waiter = command.Waiter
                }
            };
        }
    }
}