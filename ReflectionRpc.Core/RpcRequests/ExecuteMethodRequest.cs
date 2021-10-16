namespace ReflectionRpc.Core.RpcRequests
{
    public record ExecuteMethodRequest(string MethodName, object[] Arguments) : IRpcRequest;
}
