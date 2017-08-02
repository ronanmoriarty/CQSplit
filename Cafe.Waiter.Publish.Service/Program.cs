using CQRSTutorial.Publisher;
using log4net.Config;
using Topshelf;

namespace Cafe.Waiter.Publish.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlConfigurator.Configure();
            HostFactory.Run(x =>
            {
                x.Service<PublishService>(publishService =>
                {
                    publishService.ConstructUsing(Bootstrapper.CreatePublishService);
                    publishService.WhenStarted(tc => tc.Start());
                    publishService.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();

                x.SetDisplayName("CQRSTutorial Event Publishing Service");
                x.SetServiceName("cqrstutorial-event-publishing-service");
                x.SetDescription("Service to publish events queue-tables to message queues");
            });
        }
    }
}
