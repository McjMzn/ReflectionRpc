namespace ReflectionRpc.Core.Communication.RpcResponses
{
    public record HostRpcResponse(Guid HostGuid) : IRpcResponse;
}
