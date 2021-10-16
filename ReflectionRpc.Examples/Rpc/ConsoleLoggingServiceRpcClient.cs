using ReflectionRpc.Core;
using ReflectionRpc.Examples.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReflectionRpc.Examples.Rpc
{
    internal class ConsoleLoggingServiceRpcClient : RpcClientBase, IConsoleLoggingService
    {
        public ConsoleLoggingServiceRpcClient(string hostAddress, Guid hostGuid) : base(hostAddress, hostGuid)
        {
        }

        public ConsoleLoggingServiceRpcClient(string hostAddress, string tag) : base(hostAddress, tag)
        {
        }

        public string MessagePrefix
        {
            get => this.GetRemotePropertyValue(nameof(this.MessagePrefix)) as string;
            set => this.SetRemotePropertyValue(nameof(this.MessagePrefix), value);
        }

        public IConsoleLoggingServiceSettings Settings
        {
            get
            {
                var value = this.GetRemotePropertyValue(nameof(this.Settings));
                if (value is Guid guid)
                {
                    return new ConsoleLoggingServiceSettingsRpcClient(this.HostAddress, guid);
                }

                throw new Exception("Unsupported response object!");
            }

            set => throw new NotImplementedException();
        }

        public int GetNumberOfLoggedMessages()
        {
            var returned = this.ExecuteRemoteMethod(nameof(this.GetNumberOfLoggedMessages));
            return (int)Convert.ChangeType(returned, typeof(int));
        }

        public void PrintConsoleMessage(string message)
        {
            this.ExecuteRemoteMethod(nameof(this.PrintConsoleMessage), message);
        }

        public void PrintConsoleMessage(string message, int count)
        {
            this.ExecuteRemoteMethod(nameof(this.PrintConsoleMessage), message, count);
        }
    }
}
