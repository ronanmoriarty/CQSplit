using System;
using System.Diagnostics;

namespace Cafe.Waiter.AcceptanceTests
{
    public class ExternalProcess : IDisposable
    {
        private readonly string _workingDirectory;
        private readonly string _executable;
        private readonly string _arguments;
        private readonly Process _process;

        public ExternalProcess(string workingDirectory, string executable, string arguments)
        {
            _workingDirectory = workingDirectory;
            _executable = executable;
            _arguments = arguments;
            _process = new Process
            {
                StartInfo = new ProcessStartInfo(executable, arguments)
                {
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    WorkingDirectory = workingDirectory
                }
            };

            _process.OutputDataReceived += (sender, args) =>
            {
                Console.WriteLine($"Process: {executable} {arguments}: {args.Data}");
            };

            _process.ErrorDataReceived += (sender, args) =>
            {
                Console.WriteLine($"ERROR: Process: {executable} {arguments}: {args.Data}");
            };
        }

        public void Start()
        {
            Console.WriteLine($"Starting process: {_executable} {_arguments}; Working Directory: {_workingDirectory}");

            try
            {
                _process.Start();
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Exception starting process: {exception}");
                throw;
            }
        }

        public void Dispose()
        {
            Console.WriteLine($"Stopping process: {_executable} {_arguments}");
            Console.WriteLine($"Errors: {_process.StandardError.ReadToEnd()}");
            Console.WriteLine($"Process Exit Code: {_process.ExitCode}");
            _process?.Dispose();
        }
    }
}