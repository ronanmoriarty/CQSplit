using System;
using System.Collections.Generic;
using System.Linq;
using Cafe.Waiter.Commands;
using Cafe.Waiter.Contracts.Commands;
using Cafe.Waiter.Queries.DAL.Models;
using Cafe.Waiter.Queries.DAL.Repositories;

namespace Cafe.Waiter.Web.Controllers
{
    public class PlaceOrderCommandFactory : IPlaceOrderCommandFactory
    {
        private readonly IMenuRepository _menuRepository;

        public PlaceOrderCommandFactory(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        public PlaceOrderCommand Create(TabDetails tabDetails)
        {
            return new PlaceOrderCommand
            {
                Id = Guid.NewGuid(),
                AggregateId = tabDetails.Id,
                Items = GetOrderedItems(tabDetails)
            };
        }

        private List<OrderedItem> GetOrderedItems(TabDetails tabDetails)
        {
            var menu = _menuRepository.GetMenu();
            return tabDetails.Items.Select(item => Map(item, menu)).ToList();
        }

        private OrderedItem Map(TabItem item, Menu menu)
        {
            var currentMenuItem = menu.Items.Single(menuItem => menuItem.Id == item.MenuNumber);
            return new OrderedItem
            {
                Description = item.Name,
                IsDrink = item.IsDrink,
                MenuNumber = item.MenuNumber,
                Notes = item.Notes,
                Price = currentMenuItem.Price
            };
        }
    }
}