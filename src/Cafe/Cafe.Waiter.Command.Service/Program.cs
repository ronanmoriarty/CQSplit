namespace Cafe.Waiter.Command.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            Bootstrapper.Start();
            var service = Container.Instance.Resolve<WaiterCommandService>();
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
