using System;

namespace Cafe.Waiter.EventProjecting.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            Bootstrapper.Start();
            var service = Container.Instance.Resolve<EventProjectingService>();

            AppDomain.CurrentDomain.ProcessExit += (sender, eventArgs) =>
            {
                service.Stop();
            };

            service.Start();
        }
    }
}
