using Castle.Windsor.Installer;

namespace Cafe.Waiter.Publish.Service
{
    public class Bootstrapper
    {
        public static void Initialize()
        {
            Container.Instance.Install(FromAssembly.This());
        }
    }
}