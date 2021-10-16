using ReflectionRpc.Core.Serialization;

namespace ReflectionRpc.Core.RpcResponses
{
    public record SimpleRpcResponse : IRpcResponse
    {
        public object Value { get; }

        public SimpleRpcResponse(object value)
        {
            this.Value = ValueWrapper.WrapIfRequired(value);
        }
    }
}
