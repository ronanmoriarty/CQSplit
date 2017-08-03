using CQRSTutorial.Publisher;
using Topshelf;

namespace Cafe.Waiter.Publish.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            Bootstrapper.Initialize();

            HostFactory.Run(x =>
            {
                x.Service<PublishService>(publishService =>
                {
                    publishService.ConstructUsing(Container.Instance.Resolve<PublishService>);
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
