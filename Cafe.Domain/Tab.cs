using System;
using System.Collections.Generic;
using System.Linq;
using Cafe.Domain.Commands;
using Cafe.Domain.Events;
using Cafe.Domain.Exceptions;
using CQRSTutorial.Core;

namespace Cafe.Domain
{
    public class Tab
        : ICommandHandler<OpenTab>
        , ICommandHandler<PlaceOrder>
        , ICommandHandler<MarkDrinksServed>
        , IApplyEvent<TabOpened>
        , IApplyEvent<DrinksOrdered>
        , IApplyEvent<DrinksServed>
    {
        private bool _isOpened;
        private readonly List<int> _drinksAwaitingServing = new List<int>();

        public IEnumerable<IEvent> Handle(OpenTab command)
        {
            Console.WriteLine("Handling OpenTab command...");
            return new IEvent[]
            {
                new TabOpened
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

            var events = new List<IEvent>();
            events.AddRange(GetEventForAnyDrinks(command));
            events.AddRange(GetEventForAnyFood(command));
            return events;
        }

        public IEnumerable<IEvent> Handle(MarkDrinksServed command)
        {
            var menuNumbers = command.MenuNumbers;
            UpdateDrinksAwaitingServing(menuNumbers);

            return new IEvent[]
            {
                new DrinksServed
                {
                    Id = command.TabId,
                    MenuNumbers = menuNumbers
                }
            };
        }

        private void UpdateDrinksAwaitingServing(List<int> menuNumbers)
        {
            foreach (var menuNumber in menuNumbers)
            {
                if (_drinksAwaitingServing.Contains(menuNumber))
                {
                    _drinksAwaitingServing.Remove(menuNumber);
                }
                else
                {
                    throw new DrinksNotOutstanding();
                }
            }
        }

        private IEnumerable<IEvent> GetEventForAnyFood(PlaceOrder command)
        {
            var food = command.Items.Where(i => !i.IsDrink).ToList();
            if (!food.Any())
            {
                return new IEvent[] {};
            }

            return new IEvent[]
            {
                new FoodOrdered
                {
                    Id = command.TabId,
                    Items = food
                }
            };
        }

        private IEnumerable<IEvent> GetEventForAnyDrinks(PlaceOrder command)
        {
            var drinks = command.Items.Where(i => i.IsDrink).ToList();
            if (!drinks.Any())
            {
                return new IEvent[] { };
            }

            return new IEvent[]
            {
                new DrinksOrdered
                {
                    Id = command.TabId,
                    Items = drinks
                }
            };
        }

        public void Apply(TabOpened @event)
        {
            _isOpened = true;
        }

        public void Apply(DrinksOrdered @event)
        {
            _drinksAwaitingServing.AddRange(@event.Items.Select(x => x.MenuNumber));
        }

        public void Apply(DrinksServed @event)
        {
            UpdateDrinksAwaitingServing(@event.MenuNumbers);
        }
    }
}