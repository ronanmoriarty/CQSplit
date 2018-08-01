using System.Reflection;
using Castle.Windsor.Installer;
using NLog;

namespace Cafe.Waiter.Command.Service
{
    public class Bootstrapper
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public static void Start()
        {
            Logger.Debug("Bootstrapping...");
            Container.Instance.Install(FromAssembly.Instance(Assembly.GetExecutingAssembly()));
        }
    }
}