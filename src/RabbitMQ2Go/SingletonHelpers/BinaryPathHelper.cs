using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace RabbitMQ2Go.SingletonHelpers
{
    public interface IBinaryPathHelper
    {
        string ErlangRoot { get; set; }
        string RabbitMqRoot { get; set; }
    }
    public class BinaryPathHelper : IBinaryPathHelper
    {
        public BinaryPathHelper(bool tryToFindPaths = true)
        {
            if (!tryToFindPaths) return;

            var packageDir = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

            try
            {
                while (Path.GetFileName(packageDir) != "packages")
                {
                    packageDir = Directory.GetDirectories(packageDir ?? "").FirstOrDefault(subDir => Path.GetFileName(subDir) == "packages") ?? Path.GetDirectoryName(packageDir);
                }

                ErlangRoot = Path.Combine(packageDir, "Erlang.OTP.64bit.18.3.0", "tools", "binary");
                RabbitMqRoot = Path.Combine(packageDir, "RabbitMQ.3.6.2", "tools", "binary");
            }
            catch (ArgumentException)
            {
            }
        }
        public string ErlangRoot { get; set; }
        public string RabbitMqRoot { get; set; }
    }
}
