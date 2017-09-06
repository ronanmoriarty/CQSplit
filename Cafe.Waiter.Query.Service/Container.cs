using Castle.Windsor;

namespace Cafe.Waiter.Query.Service
{
    public static class Container
    {
        static Container()
        {
            Instance = new WindsorContainer();
        }

        public static IWindsorContainer Instance { get; }
    }
}