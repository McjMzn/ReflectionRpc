namespace ReflectionRpc.Http.Requests
{
    public record HttpRpcGetPropertyRequest(string PropertyName) : IHttpRpcRequest;
}
