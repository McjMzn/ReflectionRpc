using ReflectionRpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReflectionRpc.Local
{
    public class LocalRpcHost : RpcHost
    {
        public LocalRpcHost(object target)
            : base(target)
        {
        }
    }
}
