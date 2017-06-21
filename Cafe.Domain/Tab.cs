using System;
using System.Collections.Generic;
using System.Linq;
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
            AddEventForAnyDrinks(command, events);
            AddEventForAnyFood(command, events);

            return events;
        }

        private void AddEventForAnyDrinks(PlaceOrder command, List<IEvent> events)
        {
            var drinks = GetAllDrinks(command);
            if (drinks.Any())
            {
                var drinksOrdered = new DrinksOrdered
                {
                    Id = command.TabId,
                    Items = drinks.ToList()
                };
                events.Add(drinksOrdered);
            }
        }

        private void AddEventForAnyFood(PlaceOrder command, List<IEvent> events)
        {
            var food = GetAllFood(command);
            if (food.Any())
            {
                var foodOrdered = new FoodOrdered
                {
                    Id = command.TabId,
                    Items = food.ToList()
                };
                events.Add(foodOrdered);
            }
        }

        private IList<OrderedItem> GetAllFood(PlaceOrder command)
        {
            var food = command.Items.Where(item => !item.IsDrink);
            var foodList = food as IList<OrderedItem> ?? food.ToList();
            return foodList;
        }

        private IList<OrderedItem> GetAllDrinks(PlaceOrder command)
        {
            var drinks = command.Items.Where(item => item.IsDrink);
            var drinksList = drinks as IList<OrderedItem> ?? drinks.ToList();
            return drinksList;
        }

        public void Apply(TabOpened @event)
        {
            _isOpened = true;
        }
    }
}