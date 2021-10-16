using SimpleRpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReflectionRpc.Local
{
    public class LocalRpcHost<T> : RpcHostBase<T>
    {
        public LocalRpcHost(T target)
            : base(target)
        {
        }
    }
}
