using ReflectionRpc.Core.Communication;
using ReflectionRpc.Core.Communication.RpcResponses;
using ReflectionRpc.Core.Communication.Serialization;
using ReflectionRpc.Core.Interfaces;
using RestSharp;

namespace ReflectionRpc.Core
{
    public class RpcClient : IRpcClient
    {
        private RestClient restClient;

        protected string HostAddress { get; }
        protected Guid HostGuid { get; }

        private RpcClient(string hostAddress)
        {
            this.HostAddress = hostAddress;
            this.restClient = new RestClient(hostAddress);
        }

        public RpcClient(string hostAddress, string tag)
            : this(hostAddress)
        {
            var request = new RestRequest($"rpc/hosts/tagged/{tag}", Method.GET);
            this.HostGuid = this.restClient.Execute<Guid>(request).Data;
        }

        public RpcClient(string hostAddress, Guid hostGuid)
            : this(hostAddress)
        {
            this.HostGuid = hostGuid;
        }

        public object ExecuteRemoteMethod(string methodName, params object[] arguments)
        {
            return this.SendServerRequest($"rpc/hosts/{HostGuid}/methods/{methodName}", Method.POST, true, arguments);
        }

        public object GetRemotePropertyValue(string propertyName)
        {
            return this.SendServerRequest($"rpc/hosts/{HostGuid}/properties/{propertyName}", Method.GET);
        }

        public void SetRemotePropertyValue(string propertyName, object value)
        {
            this.SendServerRequest($"rpc/hosts/{this.HostGuid}/properties/{propertyName}", Method.POST, true, value);
        }

        private object SendServerRequest(string url, Method method, bool containsPaylod = false, object payload = null)
        {
            var request = new RestRequest(url, method);
            if (containsPaylod)
            {
                var serializedPayload = RpcJsonSerializer.Serialize(payload);
                request.AddJsonBody(serializedPayload);
            }

            var serverResponse = this.restClient.Execute<string>(request);
            
            return this.HandleServerResponse(serverResponse);
        }

        private object HandleServerResponse(IRestResponse<string> serverResponse)
        {
            var serializedResponse = serverResponse.Data;
            var rpcResponse = RpcJsonSerializer.DeserializeRpcResponse(serializedResponse);
            return rpcResponse switch
            {
                VoidRpcResponse voidResponse => null,
                ValueRpcResponse valueResponse => ValueWrapper.UnwrapIfPossible(valueResponse.Value),
                HostRpcResponse hostResponse => new RpcClient(this.HostAddress, hostResponse.HostGuid),
                ExceptionRpcResponse exceptionResponse => throw new Exception($"{exceptionResponse.ExceptionTypeName}{Environment.NewLine}{exceptionResponse.ExceptionMessage}{Environment.NewLine}{exceptionResponse.StackTrace}."),
                _ => rpcResponse//new NotSupportedException($"Response of type {rpcResponse.GetType().Name} is not supported!")
            };
        }
    }
}
