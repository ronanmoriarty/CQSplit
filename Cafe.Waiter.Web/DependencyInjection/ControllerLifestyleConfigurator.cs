namespace Cafe.Waiter.Web.DependencyInjection
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