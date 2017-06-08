using System;
using System.Collections.Generic;
using Cafe.Domain.Commands;
using Cafe.Domain.Events;
using Cafe.Domain.Exceptions;

namespace Cafe.Domain
{
    public class Tab : ICommandHandler<OpenTab>, ICommandHandler<PlaceOrder>, IApplyEvent<TabOpened>
    {
        private bool _isOpened;

        public IEnumerable<IEvent> Handle(OpenTab command)
        {
            Console.WriteLine("Handling OpenTab command...");
            return new IEvent[]
            { new TabOpened
                {
                    Id = command.TabId,
                    TableNumber = command.TableNumber,
                    Waiter = command.Waiter
                }
            };
        }

        public IEnumerable<IEvent> Handle(PlaceOrder command)
        {
            Console.WriteLine("Handling PlaceOrder command...");
            if (!_isOpened)
            {
                throw new TabNotOpen();
            }

            return new IEvent[]
            { new FoodOrdered
                {
                    Id = command.TabId,
                    Items = command.Items
                }
            };
        }

        public IEnumerable<IEvent> Handle(object command)
        {
            var errorMessage = "Could not find suitable handler";
            Console.WriteLine(errorMessage);
            throw new Exception(errorMessage);
        }

        public void Apply(TabOpened @event)
        {
            _isOpened = true;
        }
    }
}