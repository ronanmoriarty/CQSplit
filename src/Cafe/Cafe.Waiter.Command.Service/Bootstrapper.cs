using System.Reflection;
using Castle.Windsor.Installer;
using NLog;

namespace Cafe.Waiter.Command.Service
{
    public class Bootstrapper
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        private static bool _started;
        private static readonly object LockObj = new object();

        public static void Start()
        {
            lock (LockObj)
            {
                if (!_started)
                {
                    Logger.Debug("Bootstrapping...");
                    Container.Instance.Install(FromAssembly.Instance(Assembly.GetExecutingAssembly()));
                    _started = true;
                }
            }
        }
    }
}