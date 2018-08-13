using System;
using System.Collections.Generic;
using System.Linq;
using Cafe.Waiter.Contracts.Commands;
using Cafe.Waiter.Events;
using CQSplit.Core;
using NLog;

namespace Cafe.Waiter.Domain
{
    public class Tab : Aggregate,
        ICommandHandler<IPlaceOrderCommand>
        , ICommandHandler<IMarkDrinksServedCommand>
        , ICommandHandler<IMarkFoodServedCommand>
        , ICommandHandler<ICloseTabCommand>
        , IApplyEvent<DrinksOrdered>
        , IApplyEvent<DrinksServed>
        , IApplyEvent<FoodOrdered>
        , IApplyEvent<FoodServed>
    {
        private readonly List<OrderedItem> _drinksAwaitingServing = new List<OrderedItem>();
        private readonly List<OrderedItem> _foodAwaitingServing = new List<OrderedItem>();
        private decimal _totalValueOfServedItems;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public IEnumerable<IEvent> Handle(IPlaceOrderCommand command)
        {
            _logger.Info("Handling PlaceOrder command...");
            var events = new List<IEvent>();
            events.AddRange(GetEventForAnyDrinksOrdered(command));
            events.AddRange(GetEventForAnyFoodOrdered(command));
            return events;
        }

        public IEnumerable<IEvent> Handle(IMarkFoodServedCommand command)
        {
            if (!AllItemsBeingServedWereOrdered(command.MenuNumbers, _foodAwaitingServing))
            {
                return new IEvent[]
                {
                    new FoodNotOutstanding
                    {
                        Id = Guid.NewGuid(),
                        AggregateId = command.AggregateId,
                        CommandId = command.Id
                    }
                };
            }

            UpdateItemsAwaitingServing(command.MenuNumbers, _foodAwaitingServing);

            return new IEvent[]
            {
                new FoodServed
                {
                    Id = Guid.NewGuid(),
                    CommandId = command.Id,
                    AggregateId = command.AggregateId,
                    MenuNumbers = command.MenuNumbers
                }
            };
        }

        public IEnumerable<IEvent> Handle(IMarkDrinksServedCommand command)
        {
            if (!AllItemsBeingServedWereOrdered(command.MenuNumbers, _drinksAwaitingServing))
            {
                return new IEvent[]
                {
                    new DrinksNotOutstanding
                    {
                        Id = Guid.NewGuid(),
                        AggregateId = command.AggregateId,
                        CommandId = command.Id
                    }
                };
            }

            UpdateItemsAwaitingServing(command.MenuNumbers, _drinksAwaitingServing);

            return new IEvent[]
            {
                new DrinksServed
                {
                    Id = Guid.NewGuid(),
                    CommandId = command.Id,
                    AggregateId = command.AggregateId,
                    MenuNumbers = command.MenuNumbers
                }
            };
        }

        public IEnumerable<IEvent> Handle(ICloseTabCommand command)
        {
            return new IEvent[]
            {
                new TabClosed
                {
                    Id = Guid.NewGuid(),
                    CommandId = command.Id,
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

        private IEnumerable<IEvent> GetEventForAnyFoodOrdered(IPlaceOrderCommand command)
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
                    Id = Guid.NewGuid(),
                    CommandId = command.Id,
                    AggregateId = command.AggregateId,
                    Items = food
                }
            };
        }

        private IEnumerable<IEvent> GetEventForAnyDrinksOrdered(IPlaceOrderCommand command)
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
                    Id = Guid.NewGuid(),
                    CommandId = command.Id,
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