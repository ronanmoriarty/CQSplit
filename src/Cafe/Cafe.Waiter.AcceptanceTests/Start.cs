using System.Collections.Generic;
using System.Linq;

namespace Cafe.Waiter.AcceptanceTests
{
    public class Start
    {
        private static readonly List<Executable> Executables = new List<Executable>
        {
            new Executable($".\\src\\Cafe\\Cafe.Waiter.Web\\", $"bin\\{Configuration.Name}\\netcoreapp2.0\\Cafe.Waiter.Web.dll"),
            new Executable($".\\src\\Cafe\\Cafe.Waiter.Command.Service\\bin\\{Configuration.Name}\\netcoreapp2.0\\", "Cafe.Waiter.Command.Service.dll")
        };

        public static IEnumerable<ExternalProcess> AllWaiterServices(int timeoutInMilliseconds = 10000)
        {
            return Executables.Select(executable =>
            {
                var externalProcess = new ExternalProcess(executable.WorkingDirectory, "dotnet", executable.Name, timeoutInMilliseconds);
                externalProcess.Start();
                return externalProcess;
            }).ToList();
        }
    }
}