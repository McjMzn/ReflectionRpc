using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ReflectionRpc.Core.RpcResponses;
using ReflectionRpc.Core.Serialization;
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
            var request = new RestRequest($"rpc/hosts/{HostGuid}/methods/{methodName}", Method.POST);
            if (arguments.Length > 0)
            {
                var payload = RpcJsonSerializer.Serialize(arguments);
                request.AddJsonBody(payload);
            }

            var response = this.restClient.Execute<string>(request);
            var rpcResponse = RpcJsonSerializer.DeserializeRpcResult(response.Data);

            return this.ExtractObjectFromResponse(rpcResponse);
        }

        public object GetRemotePropertyValue(string propertyName)
        {
            var request = new RestRequest($"rpc/hosts/{HostGuid}/properties/{propertyName}", Method.GET);
            var response = restClient.Execute<string>(request);
            var rpcResponse = RpcJsonSerializer.DeserializeRpcResult(response.Data);

            return this.ExtractObjectFromResponse(rpcResponse);
        }

        public void SetRemotePropertyValue(string propertyName, object value)
        {
            var payload = RpcJsonSerializer.Serialize(value);
            var request = new RestRequest($"rpc/hosts/{this.HostGuid}/properties/{propertyName}", Method.POST);
            request.AddJsonBody(payload);

            this.restClient.Execute(request);
        }

        private object ExtractObjectFromResponse(IRpcResponse response)
        {
            switch (response)
            {
                case VoidRpcResponse:
                    return null;

                case SimpleRpcResponse simpleResponse:
                    return ValueWrapper.UnwrapIfPossible(simpleResponse.Value);

                case HostRpcResponse hostResponse:
                    return new RpcClient(this.HostAddress, hostResponse.HostGuid);
                default:
                    throw new NotSupportedException($"Response of type {response.GetType().Name} not supported!");
            }
        }
    }
}
