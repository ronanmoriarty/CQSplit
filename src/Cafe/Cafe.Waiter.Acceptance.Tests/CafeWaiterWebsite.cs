namespace Cafe.Waiter.Acceptance.Tests
{
    public class CafeWaiterWebsite
    {
        public static TabBuilder CreateTab { get; } = new TabBuilder(new ChromeDriverFactory());
    }
}