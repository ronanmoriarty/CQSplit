using Topshelf;

namespace Cafe.Waiter.Query.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            Bootstrapper.Start();

            HostFactory.Run(x =>
            {
                x.Service<WaiterQueryService>(waiterService =>
                {
                    waiterService.ConstructUsing(Container.Instance.Resolve<WaiterQueryService>);
                    waiterService.WhenStarted(tc => tc.Start());
                    waiterService.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();

                x.SetDisplayName("CQRSTutorial Waiter Command Service");
                x.SetServiceName("cqrstutorial-waiter-command-service");
                x.SetDescription("Service to handle waiter commands");
            });
        }
    }
}
