using Cafe.Domain.Events;
using Cafe.Waiter.Contracts.Commands;
using CQRSTutorial.Core;

namespace Cafe.Domain
{
    public interface ITab
        : ICommandHandler<IPlaceOrderCommand>
            , ICommandHandler<IMarkDrinksServedCommand>
            , ICommandHandler<IMarkFoodServedCommand>
            , ICommandHandler<ICloseTabCommand>
            , IApplyEvent<DrinksOrdered>
            , IApplyEvent<DrinksServed>
            , IApplyEvent<FoodOrdered>
            , IApplyEvent<FoodServed>
    {
    }
}