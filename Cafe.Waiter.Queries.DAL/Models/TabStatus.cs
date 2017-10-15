namespace Cafe.Waiter.Queries.DAL.Models
{
    public class TabStatus
    {
        public int Id { get; set; }
        public string Description { get; set; }

        public static TabStatus Seated = new TabStatus(1, "Seated");
        public static TabStatus OrderPlaced = new TabStatus(2, "Order Placed");
        public static TabStatus AllDrinksServed = new TabStatus(3, "All Drinks Served");
        public static TabStatus AllFoodAndDrinksServed = new TabStatus(4, "All Food and Drinks Served");
        public static TabStatus DessertOrdered = new TabStatus(5, "Dessert Ordered");
        public static TabStatus AllDessertsServed = new TabStatus(6, "All Desserts Served");
        public static TabStatus BillRequested = new TabStatus(7, "Bill Requested");

        public TabStatus()
        {
            // required for deserialisation purposes (public setters also required for same reason).
        }

        private TabStatus(int id, string description)
        {
            Id = id;
            Description = description;
        }

        protected bool Equals(TabStatus other)
        {
            return Id == other.Id && string.Equals(Description, other.Description);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TabStatus)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Id * 397) ^ (Description != null ? Description.GetHashCode() : 0);
            }
        }

        public static bool operator ==(TabStatus left, TabStatus right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(TabStatus left, TabStatus right)
        {
            return !Equals(left, right);
        }
    }
}