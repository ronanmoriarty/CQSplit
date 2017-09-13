namespace Cafe.Waiter.Contracts.QueryResponses
{
    public interface ITab
    {
        string Waiter { get; set; }
        int TableNumber { get; set; }
    }
}