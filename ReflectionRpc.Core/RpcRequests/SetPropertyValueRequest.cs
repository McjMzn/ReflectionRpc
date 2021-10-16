namespace ReflectionRpc.Core.RpcRequests
{
    public record SetPropertyValueRequest(string PropertyName, object Value) : IRpcRequest;
}
