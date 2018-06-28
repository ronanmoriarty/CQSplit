namespace Cafe.Waiter.AcceptanceTests
{
    public class Configuration
    {
#if DEBUG
        public const string Name = "Debug";
#else
        public const string Name = "Release";
#endif
    }
}