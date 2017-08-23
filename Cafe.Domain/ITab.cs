using Cafe.Domain.Events;
using Cafe.Waiter.Contracts;
using Cafe.Waiter.Contracts.Commands;
using CQRSTutorial.Core;

namespace Cafe.Domain
{
    public interface ITab
        : ICommandHandler<IPlaceOrder>
            , ICommandHandler<IMarkDrinksServed>
            , ICommandHandler<IMarkFoodServed>
            , ICommandHandler<ICloseTab>
            , IApplyEvent<DrinksOrdered>
            , IApplyEvent<DrinksServed>
            , IApplyEvent<FoodOrdered>
            , IApplyEvent<FoodServed>
    {
    }
}