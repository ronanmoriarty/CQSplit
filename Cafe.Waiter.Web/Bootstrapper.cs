using System.Web.Mvc;
using Castle.Windsor.Installer;
using log4net.Config;

namespace Cafe.Waiter.Web
{
    public static class Bootstrapper
    {
        public static void Start()
        {
            XmlConfigurator.Configure();
            Container.Instance.Install(FromAssembly.This());
            DependencyResolver.SetResolver(new WindsorDependencyResolver(Container.Instance));
        }
    }
}