using Topshelf;

namespace Cafe.Waiter.EventProjecting.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            Bootstrapper.Start();

            HostFactory.Run(x =>
            {
                x.Service<EventProjectingService>(eventProjectingService =>
                {
                    eventProjectingService.ConstructUsing(Container.Instance.Resolve<EventProjectingService>);
                    eventProjectingService.WhenStarted(tc => tc.Start());
                    eventProjectingService.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();

                x.SetDisplayName("CQRSTutorial Waiter Event Projecting Service");
                x.SetServiceName("cqrstutorial-waiter-event-projecting-service");
                x.SetDescription("Service to project waiter events into the waiter read model");
            });
        }
    }
}
