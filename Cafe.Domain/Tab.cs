﻿using System;
using System.Collections.Generic;
using System.Linq;
using Cafe.Domain.Commands;
using Cafe.Domain.Events;
using Cafe.Domain.Exceptions;
using CQRSTutorial.Core;

namespace Cafe.Domain
{
    public class Tab : Aggregate
        , ICommandHandler<PlaceOrder>
        , ICommandHandler<MarkDrinksServed>
        , ICommandHandler<MarkFoodServed>
        , ICommandHandler<CloseTab>
        , IApplyEvent<DrinksOrdered>
        , IApplyEvent<DrinksServed>
        , IApplyEvent<FoodOrdered>
        , IApplyEvent<FoodServed>
    {
        private readonly List<OrderedItem> _drinksAwaitingServing = new List<OrderedItem>();
        private readonly List<OrderedItem> _foodAwaitingServing = new List<OrderedItem>();
        private decimal _totalValueOfServedItems;

        public IEnumerable<IEvent> Handle(PlaceOrder command)
        {
            Console.WriteLine("Handling PlaceOrder command...");
            var events = new List<IEvent>();
            events.AddRange(GetEventForAnyDrinksOrdered(command));
            events.AddRange(GetEventForAnyFoodOrdered(command));
            return events;
        }

        public IEnumerable<IEvent> Handle(MarkFoodServed command)
        {
            if (!AllItemsBeingServedWereOrdered(command.MenuNumbers, _foodAwaitingServing))
            {
                return new IEvent[] { new FoodNotOutstanding() };
            }

            UpdateItemsAwaitingServing(command.MenuNumbers, _foodAwaitingServing);

            return new IEvent[]
            {
                new FoodServed
                {
                    AggregateId = command.AggregateId,
                    MenuNumbers = command.MenuNumbers
                }
            };
        }

        public IEnumerable<IEvent> Handle(MarkDrinksServed command)
        {
            if (!AllItemsBeingServedWereOrdered(command.MenuNumbers, _drinksAwaitingServing))
            {
                return new IEvent[] { new DrinksNotOutstanding() };
            }

            UpdateItemsAwaitingServing(command.MenuNumbers, _drinksAwaitingServing);

            return new IEvent[]
            {
                new DrinksServed
                {
                    AggregateId = command.AggregateId,
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
                    AggregateId = command.AggregateId,
                    AmountPaid = command.AmountPaid,
                    OrderValue = _totalValueOfServedItems,
                    TipValue = command.AmountPaid - _totalValueOfServedItems
                }
            };
        }

        private bool AllItemsBeingServedWereOrdered(List<int> menuNumbers, List<OrderedItem> itemsAwaitingServing)
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

        private IEnumerable<IEvent> GetEventForAnyFoodOrdered(PlaceOrder command)
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
                    AggregateId = command.AggregateId,
                    Items = food
                }
            };
        }

        private IEnumerable<IEvent> GetEventForAnyDrinksOrdered(PlaceOrder command)
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
                    AggregateId = command.AggregateId,
                    Items = drinks
                }
            };
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