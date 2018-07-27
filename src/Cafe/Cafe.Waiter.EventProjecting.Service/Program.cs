using System;
using log4net;

namespace Cafe.Waiter.EventProjecting.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            Bootstrapper.Start();
            var logger = LogManager.GetLogger(typeof(Program));
            try
            {
                var service = Container.Instance.Resolve<EventProjectingService>();

                AppDomain.CurrentDomain.ProcessExit += (sender, eventArgs) =>
                {
                    service.Stop();
                };

                service.Start();
            }
            catch (Exception exception)
            {
                logger.Error(exception);
                throw;
            }
        }
    }
}
