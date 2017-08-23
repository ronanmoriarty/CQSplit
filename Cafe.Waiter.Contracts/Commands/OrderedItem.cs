namespace Cafe.Waiter.Contracts.Commands
{
    public class OrderedItem
    {
        public int MenuNumber { get; set; }
        public string Description { get; set; }
        public bool IsDrink { get; set; }
        public decimal Price { get; set; }
    }
}