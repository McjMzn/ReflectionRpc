using ReflectionRpc.Core;
using ReflectionRpc.Examples.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReflectionRpc.Examples.Rpc
{
    internal class ConsoleLoggingServiceSettingsRpcClient : RpcClientBase, IConsoleLoggingServiceSettings
    {
        public ConsoleLoggingServiceSettingsRpcClient(string hostAddress, Guid hostGuid) : base(hostAddress, hostGuid)
        {
        }

        public ConsoleColor BackgroundColor
        {
            get => Enum.Parse<ConsoleColor>((string)this.GetRemotePropertyValue(nameof(this.BackgroundColor)));
            set => this.SetRemotePropertyValue(nameof(this.BackgroundColor), value);
        }

        public ConsoleColor ForegroundColor
        {
            get => Enum.Parse<ConsoleColor>((string)this.GetRemotePropertyValue(nameof(this.ForegroundColor)));
            set => this.SetRemotePropertyValue(nameof(this.ForegroundColor), value);
        }
    }
}
