using ImpromptuInterface;
using ReflectionRpc.Core;
using ReflectionRpc.Core.RpcResponses;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReflectionRpc.Examples
{
    public class DynamicRpcClient : DynamicObject
    {
        RpcClientBase rpcClient;

        public DynamicRpcClient(RpcClientBase rpcClient)
        {
            this.rpcClient = rpcClient;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object?[]? args, out object? result)
        {
            result = this.rpcClient.ExecuteRemoteMethod(binder.Name, args);
            return true;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object? result)
        {
            result = this.rpcClient.GetRemotePropertyValue(binder.Name);
            if (result is RpcClientBase rpcClientResult)
            {
                result = new DynamicRpcClient(rpcClientResult);
            }

            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object? value)
        {
            this.rpcClient.SetRemotePropertyValue(binder.Name, value);
            return true;
        }
    }
}
