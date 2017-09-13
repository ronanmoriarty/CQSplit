namespace Cafe.Waiter.Queries.DAL.Models
{
    public class TabItem
    {
        public int MenuNumber { get; set; }
        public string Notes { get; set; }
        public bool IsDrink { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}