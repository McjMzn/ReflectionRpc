using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ReflectionRpc.Core.RpcResponses;
using RestSharp;

namespace ReflectionRpc.Core
{
    public class RpcClientBase : IRpcClient
    {
        private RestClient restClient;

        protected string HostAddress { get; }
        protected Guid HostGuid { get; }

        private RpcClientBase(string hostAddress)
        {
            this.HostAddress = hostAddress;
            this.restClient = new RestClient(hostAddress);
        }

        public RpcClientBase(string hostAddress, string tag)
            : this(hostAddress)
        {
            var request = new RestRequest($"rpc/hosts/tagged/{tag}", Method.GET);
            this.HostGuid = this.restClient.Execute<Guid>(request).Data;
        }

        public RpcClientBase(string hostAddress, Guid hostGuid)
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
            var rpcResult = RpcJsonSerializer.DeserializeRpcResult(response.Data);

            if (rpcResult is VoidRpcResponse voidResult)
            {
                return null;
            }

            if (rpcResult is SimpleRpcResponse simpleResult)
            {
                return simpleResult.Value;
            }

            return (rpcResult as HostRpcResponse).HostGuid;
        }

        public object GetRemotePropertyValue(string propertyName)
        {
            var request = new RestRequest($"rpc/hosts/{HostGuid}/properties/{propertyName}", Method.GET);
            var response = restClient.Execute<string>(request);
            var rpcResult = RpcJsonSerializer.DeserializeRpcResult(response.Data);


            if (rpcResult is VoidRpcResponse voidResult)
            {
                return null;
            }

            if (rpcResult is SimpleRpcResponse simpleResult)
            {
                return simpleResult.Value;
            }

            return (rpcResult as HostRpcResponse).HostGuid;
        }

        public void SetRemotePropertyValue(string propertyName, object value)
        {
            var payload = RpcJsonSerializer.Serialize(value);
            var request = new RestRequest($"rpc/hosts/{this.HostGuid}/properties/{propertyName}", Method.POST);
            request.AddJsonBody(payload);

            this.restClient.Execute(request);
        }
    }
}
