using Cafe.Waiter.Commands;
using Cafe.Waiter.Queries.DAL.Models;

namespace Cafe.Waiter.Web.Controllers
{
    public interface IPlaceOrderCommandFactory
    {
        PlaceOrderCommand Create(TabDetails tabDetails);
    }
}