using Castle.Windsor.Installer;
using log4net.Config;

namespace Cafe.Waiter.Publish.Service
{
    public class Bootstrapper
    {
        public static void Start()
        {
            XmlConfigurator.Configure();
            Container.Instance.Install(FromAssembly.This());
        }
    }
}