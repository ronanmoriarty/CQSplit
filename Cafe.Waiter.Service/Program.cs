using Topshelf;

namespace Cafe.Waiter.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            Bootstrapper.Start();

            HostFactory.Run(x =>
            {
                x.Service<WaiterService>(waiterService =>
                {
                    waiterService.ConstructUsing(Container.Instance.Resolve<WaiterService>);
                    waiterService.WhenStarted(tc => tc.Start());
                    waiterService.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();

                x.SetDisplayName("CQRSTutorial Waiter Service");
                x.SetServiceName("cqrstutorial-waiter-service");
                x.SetDescription("Service to handle waiter commands");
            });
        }
    }
}
