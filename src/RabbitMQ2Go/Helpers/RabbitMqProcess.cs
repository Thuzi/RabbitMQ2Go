using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Holf.AllForOne;
using RabbitMQ2Go.Extensions;
using RabbitMQ2Go.SingletonHelpers;
using WindowsProcess = System.Diagnostics.Process;

namespace RabbitMQ2Go.Helpers
{
    public interface IRabbitMqProcess : IProcessWrapper
    {
    }
    public class RabbitMqProcess : ProcessWrapper, IRabbitMqProcess
    {
        private IBinaryPathHelper binaryPathHelper;
        private IRabbitMqEnvironmentVariables rabbitMqEnvironmentVariables;
        private WindowsProcess[] erlang;
        private class rabbitMqManagementProcess : ProcessWrapper
        {
            protected override bool BeforeReadyLineReader(string line)
            {
#if DEBUG
                Console.WriteLine(line);
#endif
                var returnValue = Regex.IsMatch(line, @".*Applying plugin configuration to .*");
                return returnValue;
            }
            protected override bool UserBeforeReadyLineReader
            {
                get
                {
                    return true;
                }
            }
        }

        protected override bool BeforeReadyLineReader(string line)
        {
#if DEBUG
            Console.WriteLine(line);
#endif
            var returnValue = Regex.IsMatch(line, @".*completed with \d+ plugins.*");
            return returnValue;
        }
        protected override bool UserBeforeReadyLineReader
        {
            get
            {
                return true;
            }
        }
        protected override void Disposed()
        {
            var processes = erlang;
            erlang = null;
            if (processes == null) return;

            foreach (var process in processes)
                using (process)
                {
                    if (!process.HasExited)
                    {
                        process.Kill();
                    }
                }
        }

        public RabbitMqProcess()
        {
            binaryPathHelper = new BinaryPathHelper();
            rabbitMqEnvironmentVariables = new RabbitMqEnvironmentVariables();
        }
        public IBinaryPathHelper BinaryPathHelper
        {
            get
            {
                return binaryPathHelper;
            }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                binaryPathHelper = value;
            }
        }
        public IRabbitMqEnvironmentVariables RabbitMqEnvironmentVariables
        {
            get
            {
                return rabbitMqEnvironmentVariables;
            }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                rabbitMqEnvironmentVariables = value;
            }
        }
        public override async Task Run()
        {
            if (String.IsNullOrWhiteSpace(RabbitMqEnvironmentVariables.RabbitMqPluginsDir))
            {
                RabbitMqEnvironmentVariables.RabbitMqPluginsDir =
                    Path.Combine(BinaryPathHelper.RabbitMqRoot, "plugins");
            }

            foreach (var valuePair in typeof(IRabbitMqEnvironmentVariables)
                .GetProperties()
                .Where(property => property.CanRead && property.PropertyType == typeof(string))
                .Select(property => new
                {
                    NameAtt = property.GetCustomAttribute<DisplayNameAttribute>(),
                    Property = property
                })
                .Where(property => property.NameAtt != null)
                .Select(property => new
                {
                    IoPath = property.Property.GetCustomAttribute<IoPathAttribute>(),
                    Name = property.NameAtt.DisplayName,
                    Value = (string)property.Property.GetValue(RabbitMqEnvironmentVariables)
                })
                .Where(valuePair => !String.IsNullOrEmpty(valuePair.Value)))
            {
                EnvironmentVariables[valuePair.Name] = valuePair.IoPath == null ?
                    valuePair.Value :
                    valuePair.IoPath.IsDirectory ?
                        String.Format("\"{0}\\{1}\"", valuePair.Value.MakePathForErlang(), Path.DirectorySeparatorChar) :
                        String.Format("\"{0}\"", valuePair.Value.MakePathForErlang());
            }

            EnvironmentVariables["ERLANG_HOME"] = String.Format("\"{0}\\{1}\"", BinaryPathHelper.ErlangRoot.MakePathForErlang(), Path.DirectorySeparatorChar);
            Exe = Path.Combine(BinaryPathHelper.RabbitMqRoot, "sbin", "rabbitmq-server.bat");

            try
            {
                await base.Run();

                erlang = WindowsProcess.GetProcessesByName("erl");

                if (RabbitMqEnvironmentVariables.EnableManagement)
                {
                    using (var managementProcess = new rabbitMqManagementProcess
                    {
                        Arguments = "enable rabbitmq_management",
                        EnvironmentVariables = EnvironmentVariables,
                        Exe = Path.Combine(BinaryPathHelper.RabbitMqRoot, "sbin", "rabbitmq-plugins.bat")
                    })
                    {
                        await managementProcess.Run();
                    }
                }
            }
            catch
            {
                foreach (var erl in WindowsProcess.GetProcessesByName("erl"))
                    using (erl)
                        erl.Kill();
                throw;
            }

            foreach (var process in erlang)
                process.TieLifecycleToParentProcess();
        }
    }
}
