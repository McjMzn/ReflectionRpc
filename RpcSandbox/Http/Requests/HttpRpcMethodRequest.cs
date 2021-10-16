namespace ReflectionRpc.Http.Requests
{
    public record HttpRpcMethodRequest(string MethodName, object[] Arguments) : IHttpRpcRequest;
}
