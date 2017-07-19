using Cafe.Domain;

namespace Cafe.Waiter.DAL.Tests
{
    public static class TabExtensions
    {
        public static TabInspector GetInspector(this Tab tab)
        {
            return new TabInspector(tab);
        }
    }
}