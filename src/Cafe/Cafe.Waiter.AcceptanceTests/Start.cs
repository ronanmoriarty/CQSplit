using System.Collections.Generic;
using System.Linq;

namespace Cafe.Waiter.AcceptanceTests
{
    public class Start
    {
        private static readonly List<Executable> Executables = new List<Executable>
        {
            new Executable($".\\src\\Cafe\\Cafe.Waiter.Web\\bin\\{Configuration.Name}\\netcoreapp2.0\\", "Cafe.Waiter.Web.dll")
        };

        public static IEnumerable<ExternalProcess> AllWaiterServices()
        {
            return Executables.Select(executable =>
            {
                var externalProcess = new ExternalProcess(executable.WorkingDirectory, "dotnet", executable.Name);
                externalProcess.Start();
                return externalProcess;
            });
        }
    }
}