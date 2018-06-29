using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Cafe.Waiter.AcceptanceTests
{
    public class ExternalProcess : IDisposable
    {
        private readonly string _executable;
        private readonly string _arguments;
        private readonly int _timeoutInMilliseconds;
        private readonly Process _process;
        private readonly AutoResetEvent _outputWaitHandle;
        private readonly AutoResetEvent _errorWaitHandle;

        public ExternalProcess(string workingDirectory, string executable, string arguments, int timeoutInMilliseconds)
        {
            var path = Path.Combine(workingDirectory, arguments);
            if (!File.Exists(path))
            {
                throw new ArgumentException($"Path {path} does not exist.");
            }

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
            _outputWaitHandle = new AutoResetEvent(false);
            _errorWaitHandle = new AutoResetEvent(false);
        }

        public void Start()
        {
            // Following adapted from https://stackoverflow.com/questions/139593/processstartinfo-hanging-on-waitforexit-why
            _process.OutputDataReceived += (sender, e) =>
            {
                if (e.Data == null)
                {
                    _outputWaitHandle.Set();
                }
                else
                {
                    Log(e.Data);
                }
            };
            _process.ErrorDataReceived += (sender, e) =>
            {
                if (e.Data == null)
                {
                    _errorWaitHandle.Set();
                }
                else
                {
                    Log($"ERROR: {e.Data}");
                }
            };

            try
            {
                _process.Start();
                Log($"Process {_executable} {_arguments} started.");
            }
            catch (Exception exception)
            {
                Log($"Exception starting process: {exception}");
                throw;
            }

            _process.BeginOutputReadLine();
            _process.BeginErrorReadLine();
        }

        private void Log(string message)
        {
            Console.WriteLine($"[{_process.Id}] {message}");
        }

        public void Dispose()
        {
            Console.WriteLine($"Stopping process: {_executable} {_arguments}");
            Stop();
            _process.Kill();
            _process?.Dispose();
        }

        private void Stop()
        {
            if (_process.WaitForExit(_timeoutInMilliseconds) &&
                _outputWaitHandle.WaitOne(_timeoutInMilliseconds) &&
                _errorWaitHandle.WaitOne(_timeoutInMilliseconds))
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