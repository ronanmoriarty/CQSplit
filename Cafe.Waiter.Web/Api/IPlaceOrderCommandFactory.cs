using Cafe.Waiter.Commands;
using Cafe.Waiter.Queries.DAL.Models;

namespace Cafe.Waiter.Web.Api
{
    public interface IPlaceOrderCommandFactory
    {
        PlaceOrderCommand Create(TabDetails tabDetails);
    }
}