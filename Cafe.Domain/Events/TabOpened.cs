namespace Cafe.Domain.Events
{
    public class TabOpened : ITabEvent
    {
        public int Id { get; set; }
        public int TabId { get; set; }
        public int TableNumber { get; set; }
        public string Waiter { get; set; }
    }
}