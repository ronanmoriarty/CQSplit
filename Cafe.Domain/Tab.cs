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
        , ICommandHandler<MarkFoodServed>
        , ICommandHandler<CloseTab>
        , IApplyEvent<TabOpened>
        , IApplyEvent<DrinksOrdered>
        , IApplyEvent<DrinksServed>
        , IApplyEvent<FoodOrdered>
        , IApplyEvent<FoodServed>
    {
        private bool _isOpened;
        private readonly List<OrderedItem> _drinksAwaitingServing = new List<OrderedItem>();
        private readonly List<OrderedItem> _foodAwaitingServing = new List<OrderedItem>();
        private decimal _totalValueOfServedItems;

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

        public IEnumerable<IEvent> Handle(MarkFoodServed command)
        {
            if (!AllItemsAwaitingServing(command.MenuNumbers, _foodAwaitingServing))
            {
                throw new FoodNotOutstanding();
            }

            UpdateItemsAwaitingServing(command.MenuNumbers, _foodAwaitingServing);

            return new IEvent[]
            {
                new FoodServed
                {
                    Id = command.TabId,
                    MenuNumbers = command.MenuNumbers
                }
            };
        }

        public IEnumerable<IEvent> Handle(MarkDrinksServed command)
        {
            if (!AllItemsAwaitingServing(command.MenuNumbers, _drinksAwaitingServing))
            {
                throw new DrinksNotOutstanding();
            }

            UpdateItemsAwaitingServing(command.MenuNumbers, _drinksAwaitingServing);

            return new IEvent[]
            {
                new DrinksServed
                {
                    Id = command.TabId,
                    MenuNumbers = command.MenuNumbers
                }
            };
        }

        public IEnumerable<IEvent> Handle(CloseTab command)
        {
            return new IEvent[]
            {
                new TabClosed
                {
                    Id = command.Id,
                    AmountPaid = command.AmountPaid,
                    OrderValue = _totalValueOfServedItems,
                    TipValue = command.AmountPaid - _totalValueOfServedItems
                }
            };
        }

        private bool AllItemsAwaitingServing(List<int> menuNumbers, List<OrderedItem> itemsAwaitingServing)
        {
            var currentItemsAwaitingServing = new List<OrderedItem>(itemsAwaitingServing);
            foreach (var menuNumber in menuNumbers)
            {
                if (currentItemsAwaitingServing.Any(orderedItem => orderedItem.MenuNumber == menuNumber))
                {
                    var firstItemMatchingMenuNumber = currentItemsAwaitingServing.First(orderedItem => orderedItem.MenuNumber == menuNumber);
                    currentItemsAwaitingServing.Remove(firstItemMatchingMenuNumber);
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        private IEnumerable<IEvent> GetEventForAnyFood(PlaceOrder command)
        {
            var food = command.Items.Where(i => !i.IsDrink).ToList();
            if (!food.Any())
            {
                return new IEvent[] { };
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
            _drinksAwaitingServing.AddRange(@event.Items);
        }

        public void Apply(FoodOrdered @event)
        {
            _foodAwaitingServing.AddRange(@event.Items);
        }

        public void Apply(FoodServed @event)
        {
            UpdateItemsAwaitingServing(@event.MenuNumbers, _foodAwaitingServing);
        }

        public void Apply(DrinksServed @event)
        {
            UpdateItemsAwaitingServing(@event.MenuNumbers, _drinksAwaitingServing);
        }

        private void UpdateItemsAwaitingServing(List<int> menuNumbers, List<OrderedItem> itemsAwaitingServing)
        {
            foreach (var menuNumber in menuNumbers)
            {
                var firstItemMatchingMenuNumber = itemsAwaitingServing.First(orderedItem => orderedItem.MenuNumber == menuNumber);
                itemsAwaitingServing.Remove(firstItemMatchingMenuNumber);
                _totalValueOfServedItems += firstItemMatchingMenuNumber.Price;
            }
        }
    }
}