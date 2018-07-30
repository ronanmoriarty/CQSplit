using System.Reflection;
using Castle.Windsor.Installer;

namespace Cafe.Waiter.EventProjecting.Service
{
    public class Bootstrapper
    {
        public static void Start()
        {
            Container.Instance.Install(FromAssembly.Instance(Assembly.GetExecutingAssembly()));
        }
    }
}