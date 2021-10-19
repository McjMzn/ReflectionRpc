namespace ReflectionRpc.Core.Communication.RpcRequests
{
    public record SetPropertyValueRequest(string PropertyName, object Value) : IRpcRequest;
}
