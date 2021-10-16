using System.Text;
using Newtonsoft.Json;
using ReflectionRpc.Core;
using ReflectionRpc.Http.Requests;

namespace ReflectionRpc.Http
{
    public abstract class HttpRpcClient : IRpcClient
    {
        private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
        private static readonly HttpClient HttpClient = new HttpClient();
        private string hostAddress;

        public HttpRpcClient(string hostAddress)
        {
            this.hostAddress = hostAddress;
        }

        public object ExecuteRemoteMethod(string methodName, params object[] arguments) => SendHttpRpcRequest(new HttpRpcMethodRequest(methodName, arguments));

        public object GetRemotePropertyValue(string propertyName) => SendHttpRpcRequest(new HttpRpcGetPropertyRequest(propertyName));

        public void SetRemotePropertyValue(string propertyName, object value) => SendHttpRpcRequest(new HttpRpcSetPropertyRequest(propertyName, value));

        private object SendHttpRpcRequest(IHttpRpcRequest request)
        {
            var serializedRequest = JsonConvert.SerializeObject(request, JsonSerializerSettings);
            var response = HttpClient.PostAsync(hostAddress, new StringContent(serializedRequest, Encoding.UTF8, "application/json")).Result;
            var responseContent = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject(responseContent, JsonSerializerSettings);
        }
    }
}
