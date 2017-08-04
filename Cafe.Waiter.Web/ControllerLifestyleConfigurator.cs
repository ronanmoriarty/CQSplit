namespace Cafe.Waiter.Web
{
    public static class ControllerLifestyleConfigurator
    {
        public static ILifestyle Instance { get; set; }

        static ControllerLifestyleConfigurator()
        {
            Instance = new PerWebRequestLifestyle();
        }
    }
}