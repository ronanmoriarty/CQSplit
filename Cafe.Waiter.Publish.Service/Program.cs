using System;
using CQRSTutorial.Publisher;
using log4net;
using Topshelf;

namespace Cafe.Waiter.Publish.Service
{
    class Program
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Program));

        static void Main(string[] args)
        {
            Bootstrapper.Start();

            HostFactory.Run(x =>
            {
                x.Service<PublishService>(publishService =>
                {
                    publishService.ConstructUsing(CreatePublishService);
                    publishService.WhenStarted(tc => tc.Start());
                    publishService.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();

                x.SetDisplayName("CQRSTutorial Event Publishing Service");
                x.SetServiceName("cqrstutorial-event-publishing-service");
                x.SetDescription("Service to publish events queue-tables to message queues");
            });
        }

        private static PublishService CreatePublishService()
        {
            try
            {
                return Container.Instance.Resolve<PublishService>();
            }
            catch (Exception exception)
            {
                Logger.Error("Error resolving publish service", exception);
                throw;
            }
        }
    }
}
