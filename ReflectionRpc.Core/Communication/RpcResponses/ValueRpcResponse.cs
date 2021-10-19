using ReflectionRpc.Core.Communication.Serialization;

namespace ReflectionRpc.Core.Communication.RpcResponses
{
    public record ValueRpcResponse : IRpcResponse
    {
        public object Value { get; }

        public ValueRpcResponse(object value)
        {
            Value = ValueWrapper.WrapIfRequired(value);
        }
    }
}
