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
                x.Service<WaiterQueryService>(waiterQueryService =>
                {
                    waiterQueryService.ConstructUsing(Container.Instance.Resolve<WaiterQueryService>);
                    waiterQueryService.WhenStarted(tc => tc.Start());
                    waiterQueryService.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();

                x.SetDisplayName("CQRSTutorial Waiter Query Service");
                x.SetServiceName("cqrstutorial-waiter-query-service");
                x.SetDescription("Service to update waiter read model");
            });
        }
    }
}
