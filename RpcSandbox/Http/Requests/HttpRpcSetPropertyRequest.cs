namespace ReflectionRpc.Http.Requests
{
    public record HttpRpcSetPropertyRequest(string PropertyName, object Value) : IHttpRpcRequest;
}
