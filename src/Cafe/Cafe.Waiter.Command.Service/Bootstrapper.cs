using System.IO;
using System.Reflection;
using Castle.Windsor.Installer;
using log4net;
using log4net.Config;

namespace Cafe.Waiter.Command.Service
{
    public class Bootstrapper
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Bootstrapper));

        public static void Start()
        {
            Logger.Debug("Bootstrapping...");
            var loggerRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(loggerRepository, new FileInfo("log4net.config"));
            Container.Instance.Install(FromAssembly.Instance(Assembly.GetExecutingAssembly()));
        }
    }
}