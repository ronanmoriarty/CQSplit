﻿using System;
using System.Collections.Generic;
using System.Linq;
using Cafe.Domain.Commands;
using Cafe.Domain.Events;
using Cafe.Domain.Exceptions;
using CQRSTutorial.Core;

namespace Cafe.Domain
{
    public class Tab : ICommandHandler<OpenTab>, ICommandHandler<PlaceOrder>, IApplyEvent<TabOpened>
    {
        private bool _isOpened;

        public IEnumerable<IEvent> Handle(OpenTab command)
        {
            Console.WriteLine("Handling OpenTab command...");
            yield return new TabOpened
                {
                    Id = command.TabId,
                    TableNumber = command.TableNumber,
                    Waiter = command.Waiter
                };
        }

        public IEnumerable<IEvent> Handle(PlaceOrder command)
        {
            Console.WriteLine("Handling PlaceOrder command...");
            if (!_isOpened)
            {
                throw new TabNotOpen();
            }

            foreach (var @event in GetEventForAnyDrinks(command))
            {
                yield return @event;
            }

            foreach (var @event in GetEventForAnyFood(command))
            {
                yield return @event;
            }
        }

        private static IEnumerable<IEvent> GetEventForAnyFood(PlaceOrder command)
        {
            var food = command.Items.Where(i => !i.IsDrink).ToList();
            if (food.Any())
            {
                yield return new FoodOrdered
                {
                    Id = command.TabId,
                    Items = food
                };
            }
        }

        private IEnumerable<IEvent> GetEventForAnyDrinks(PlaceOrder command)
        {
            var drinks = command.Items.Where(i => i.IsDrink).ToList();
            if (drinks.Any())
            {
                yield return new DrinksOrdered
                {
                    Id = command.TabId,
                    Items = drinks
                };
            }
        }

        public void Apply(TabOpened @event)
        {
            _isOpened = true;
        }
    }
}