using ImpromptuInterface;
using System.Dynamic;

namespace ReflectionRpc.Core
{
    public class RpcProxy : DynamicObject
    {
        RpcClient rpcClient;

        private RpcProxy(RpcClient rpcClient)
        {
            this.rpcClient = rpcClient;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object?[]? args, out object? result)
        {
            result = rpcClient.ExecuteRemoteMethod(binder.Name, args);
            return true;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object? result)
        {
            result = rpcClient.GetRemotePropertyValue(binder.Name);
            if (result is RpcClient rpcClientResult)
            {
                result = new RpcProxy(rpcClientResult);
            }

            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object? value)
        {
            rpcClient.SetRemotePropertyValue(binder.Name, value);
            return true;
        }

        public static T Create<T>(RpcClient rpcClient)
        {
            var dynamicClient = new RpcProxy(rpcClient);
            return dynamicClient.ActLike(typeof(T));
        }

        public static T Create<T>(string hostAddress, Guid hostGuid)
        {
            return Create<T>(new RpcClient(hostAddress, hostGuid));
        }

        public static T Create<T>(string hostAddress, string tag)
        {
            return Create<T>(new RpcClient(hostAddress, tag));
        }
    }
}
