using Castle.Windsor.Installer;

namespace Cafe.Waiter.Web
{
    public static class Bootstrapper
    {
        public static void Start()
        {
            Container.Instance.Install(FromAssembly.This());
        }
    }
}