using System.IO;
using System.Reflection;

namespace Cafe.Waiter.AcceptanceTests
{
    public class Executable
    {
        public string WorkingDirectory { get; }
        public string Name { get; }

        public Executable(string workingDirectory, string name)
        {
            WorkingDirectory = Path.GetFullPath(Path.Combine(Assembly.GetExecutingAssembly().Location, "..\\..\\..\\..\\..\\..\\..\\", workingDirectory));
            Name = name;
        }
    }
}