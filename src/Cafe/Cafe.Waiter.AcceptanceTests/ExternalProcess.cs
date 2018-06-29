using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Cafe.Waiter.AcceptanceTests
{
    public class ExternalProcess : IDisposable
    {
        private readonly string _workingDirectory;
        private readonly string _executable;
        private readonly string _arguments;
        private readonly int _timeoutInMilliseconds;
        private readonly Process _process;

        public ExternalProcess(string workingDirectory, string executable, string arguments, int timeoutInMilliseconds)
        {
            var path = Path.Combine(workingDirectory, arguments);
            if (!File.Exists(path))
            {
                throw new ArgumentException($"Path {path} does not exist.");
            }

            _workingDirectory = workingDirectory;
            _executable = executable;
            _arguments = arguments;
            _timeoutInMilliseconds = timeoutInMilliseconds;
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
        }

        public void Start()
        {
            // Following adapted from https://stackoverflow.com/questions/139593/processstartinfo-hanging-on-waitforexit-why
            using (var outputWaitHandle = new AutoResetEvent(false))
            {
                using (var errorWaitHandle = new AutoResetEvent(false))
                {
                    _process.OutputDataReceived += (sender, e) =>
                    {
                        if (e.Data == null)
                        {
                            outputWaitHandle.Set();
                        }
                        else
                        {
                            Console.WriteLine($"Process: {_executable} {_arguments}: {e.Data}");
                        }
                    };
                    _process.ErrorDataReceived += (sender, e) =>
                    {
                        if (e.Data == null)
                        {
                            errorWaitHandle.Set();
                        }
                        else
                        {
                            Log($"ERROR: {e.Data}");
                        }
                    };

                    try
                    {
                        _process.Start();
                        Log($"Process started with Process Id: {_process.Id}");
                    }
                    catch (Exception exception)
                    {
                        Log($"Exception starting process: {exception}");
                        throw;
                    }

                    _process.BeginOutputReadLine();
                    _process.BeginErrorReadLine();

                    if (_process.WaitForExit(_timeoutInMilliseconds) &&
                        outputWaitHandle.WaitOne(_timeoutInMilliseconds) &&
                        errorWaitHandle.WaitOne(_timeoutInMilliseconds))
                    {
                        Log($"Process Completed. Exit Code: {_process.ExitCode}");
                    }
                    else
                    {
                        Log("Process timed out.");
                    }
                }
            }
        }

        private void Log(string message)
        {
            Console.WriteLine($"Working Directory: {_workingDirectory}; Process: {_executable} {_arguments}; {message}");
        }

        public void Dispose()
        {
            Console.WriteLine($"Stopping process: {_executable} {_arguments}");
            _process.Kill();
            _process?.Dispose();
        }
    }
}