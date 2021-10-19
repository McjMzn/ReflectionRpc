namespace ReflectionRpc.Core.Communication.RpcRequests
{
    public record GetPropertyValueRequest(string PropertyName) : IRpcRequest;
}
