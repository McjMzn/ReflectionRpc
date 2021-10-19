using ReflectionRpc.Core.Communication;

namespace ReflectionRpc.Core.Interfaces
{
    public interface IRpcHostManager
    {
        RegisteredRpcHost RegisterHost(object rpcTarget, string tag = null);

        List<RegisteredRpcHost> GetHosts();

        RegisteredRpcHost GetHost(Guid guid);

        RegisteredRpcHost GetHost(string tag);

        IRpcResponse ExecuteRequest(Guid hostGuid, IRpcRequest request);
    }
}
