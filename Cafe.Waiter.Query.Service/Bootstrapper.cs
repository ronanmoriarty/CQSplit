using Castle.Windsor.Installer;

namespace Cafe.Waiter.Query.Service
{
    public static class Bootstrapper
    {
        public static void Start()
        {
            Container.Instance.Install(FromAssembly.This());
        }
    }
}