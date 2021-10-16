namespace ReflectionRpc.Core.RpcRequests
{
    public record GetPropertyValueRequest(string PropertyName) : IRpcRequest;
}
