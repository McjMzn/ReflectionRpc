namespace ReflectionRpc.Core.Communication.RpcRequests
{
    public record ExecuteMethodRequest(string MethodName, object[] Arguments) : IRpcRequest;
}
