using Castle.Windsor.Installer;
using log4net.Config;

namespace Cafe.Waiter.Publish.Service
{
    public class Bootstrapper
    {
        public static void Initialize()
        {
            XmlConfigurator.Configure();
            Container.Instance.Install(FromAssembly.This());
        }
    }
}