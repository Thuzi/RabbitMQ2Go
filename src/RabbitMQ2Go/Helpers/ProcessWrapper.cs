using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Holf.AllForOne;

namespace RabbitMQ2Go.Helpers
{
    public interface IProcessWrapper : IDisposable
    {
        Dictionary<string, object> EnvironmentVariables { get; }
        Process Process { get; }
        Task Run();
        object[] Args { get; }
        string Arguments { get; }
        string Exe { get; }
    }
    public class ProcessWrapper : IProcessWrapper
    {
        private TaskCompletionSource<bool> isReadyTask;
        private int processStarted;
        private void stdLine(string line)
        {
            var isReadyTaskCopy = isReadyTask;

            if (isReadyTaskCopy != null)
            {
                try
                {
                    if (BeforeReadyLineReader(line))
                    {
                        isReadyTaskCopy.TrySetResult(true);
                    }
                }
                catch (Exception error)
                {
                    isReadyTaskCopy.TrySetException(error);
                }
            }
        }
        private void stdErrLine(object sender, DataReceivedEventArgs e)
        {
            stdLine(e.Data ?? "");
        }
        private void stdOutLine(object sender, DataReceivedEventArgs e)
        {
            stdLine(e.Data ?? "");
        }

        protected ProcessWrapper()
        {
            EnvironmentVariables = new Dictionary<string, object>();
        }
        protected virtual bool BeforeReadyLineReader(string line)
        {
            return false;
        }
        protected virtual bool UserBeforeReadyLineReader
        {
            get
            {
                return false;
            }
        }
        protected virtual void Disposed()
        {
        }
        
        public ProcessWrapper(string exe)
            : this(exe, null)
        {
        }
        public ProcessWrapper(string exe, string arguments, params object[] args)
        {
            Args = args;
            Arguments = arguments;
            Exe = exe;
        }
        public Dictionary<string, object> EnvironmentVariables { get; private set; }
        public Process Process { get; protected set; }
        public async virtual Task Run()
        {
            if (Interlocked.CompareExchange(ref processStarted, 1, 0) == 1) throw new InvalidOperationException("The process already started.");

            isReadyTask = UserBeforeReadyLineReader ? new TaskCompletionSource<bool>() : null;
            Process = new Process();

            try
            {
                //the parameters for the program to start
                Process.EnableRaisingEvents = true;
                Process.OutputDataReceived += stdOutLine;
                Process.ErrorDataReceived += stdErrLine;
                if (!String.IsNullOrEmpty(Arguments))
                    Process.StartInfo.Arguments = String.Format(Arguments, Args ?? new object[0]);
                Process.StartInfo.CreateNoWindow = true;
                Process.StartInfo.FileName = Exe;
                Process.StartInfo.UseShellExecute = false;
                Process.StartInfo.RedirectStandardError = true;
                Process.StartInfo.RedirectStandardInput = true;
                Process.StartInfo.RedirectStandardOutput = true;
                Process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                foreach (var item in EnvironmentVariables)
                {
                    Process.StartInfo.EnvironmentVariables.Add(item.Key, Convert.ToString(item.Value));
                }

                //start the program
                Process.Start();

                Process.BeginOutputReadLine();
                Process.BeginErrorReadLine();

                Process.TieLifecycleToParentProcess();

                if (isReadyTask == null) return;
                
                await Task.WhenAny(isReadyTask.Task, Task.Delay(RunTimeout));

                if (!isReadyTask.Task.IsCompleted)
                    throw new TimeoutException(String.Format("The process failed to start within {0} second(s).", RunTimeout.TotalSeconds));
            }
            catch (Exception error)
            {
                Dispose();
                throw error.InnerException ?? error;
            }
            finally
            {
                isReadyTask = null;
            }
        }
        public object[] Args { get; protected set; }
        public string Arguments { get; protected set; }
        public string Exe { get; protected set; }
        public virtual TimeSpan RunTimeout
        {
            get
            {
                return TimeSpan.FromSeconds(30);
            }
        }
        public void Dispose()
        {
            var process = Process;
            Process = null;
            if (process == null) return;
            
            if (!process.HasExited) process.Kill();
            process.Dispose();
            processStarted = 0;
            Disposed();
        }
    }
}
