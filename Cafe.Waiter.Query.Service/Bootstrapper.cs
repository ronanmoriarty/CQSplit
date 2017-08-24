using Castle.Windsor.Installer;
using log4net.Config;

namespace Cafe.Waiter.Query.Service
{
    public static class Bootstrapper
    {
        public static void Start()
        {
            XmlConfigurator.Configure();
            Container.Instance.Install(FromAssembly.This());
        }
    }
}