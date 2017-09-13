namespace Cafe.Waiter.Queries.DAL.Models
{
    public enum TabStatus
    {
        Seated,
        OrderPlaced,
        AllDrinksServed,
        AllFoodAndDrinksServed,
        DessertOrdered,
        AllDessertsServed,
        BillRequested
        //Closed -- not required for open tabs
    }
}