using Castle.Windsor;

namespace Cafe.Waiter.Web
{
    public static class Container
    {
        static Container()
        {
            Instance = new WindsorContainer();
        }

        public static WindsorContainer Instance { get; set; }
    }
}