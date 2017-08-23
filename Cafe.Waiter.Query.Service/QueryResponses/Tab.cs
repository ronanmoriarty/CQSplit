using Cafe.Waiter.Contracts.QueryResponses;

namespace Cafe.Waiter.Query.Service.QueryResponses
{
    public class Tab : ITab
    {
        public string Waiter { get; set; }
        public int TableNumber { get; set; }
    }
}