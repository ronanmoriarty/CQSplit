namespace Cafe.Domain.Commands
{
    public class OpenTab
    {
        public int TabId { get; set; }
        public int TableNumber { get; set; }
        public string Waiter { get; set; }
    }
}