using System;

namespace Cafe.Waiter.Command.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            Bootstrapper.Start();
            var service = Container.Instance.Resolve<WaiterCommandService>();

            AppDomain.CurrentDomain.ProcessExit += (sender, eventArgs) =>
            {
                service.Stop();
            };

            service.Start();
        }
    }
}
