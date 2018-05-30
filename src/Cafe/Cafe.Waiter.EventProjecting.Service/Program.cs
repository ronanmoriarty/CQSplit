namespace Cafe.Waiter.EventProjecting.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            Bootstrapper.Start();
            var service = Container.Instance.Resolve<EventProjectingService>();
            try
            {
                service.Start();
            }
            finally
            {
                service.Stop();
            }
        }
    }
}
