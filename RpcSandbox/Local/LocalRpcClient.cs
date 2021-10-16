using SimpleRpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReflectionRpc.Local
{
    public class LocalRpcClient : IRpcClient
    {
        private readonly IRpcHost rpcHost;

        public LocalRpcClient(IRpcHost rpcHost)
        {
            this.rpcHost = rpcHost;
        }

        public object ExecuteRemoteMethod(string methodName, params object[] arguments)
        {
            return rpcHost.ExecuteMethod(methodName, arguments);
        }

        public object GetRemotePropertyValue(string propertyName)
        {
            return rpcHost.GetPropertyValue(propertyName);
        }

        public void SetRemotePropertyValue(string propertyName, object value)
        {
            rpcHost.SetPropertyValue(propertyName, value);
        }
    }
}
