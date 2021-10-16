using Microsoft.AspNetCore.Mvc;
using ReflectionRpc.Core;
using ReflectionRpc.Core.RpcResponses;

namespace ReflectionRpc.Asp
{
    public class RpcController
    {
        private RpcHostManager hostManager;

        public RpcController(RpcHostManager hostManager)
        {
            this.hostManager = hostManager;
        }

        public IResult GetHosts()
        {
            var hostInformations = this.hostManager.GetRegisteredHosts();
            return Results.Ok(hostInformations);
        }

        public IResult GetHostByTag(string tag)
        {
            var registeredHost = this.hostManager.GetRegisteredRpcHost(tag);
            return Results.Ok(registeredHost.Guid);
        }

        public IResult GetPropertyValue(Guid guid, string propertyName)
        {
            var registeredHost = this.hostManager.GetRegisteredRpcHost(guid);
            var host = registeredHost.RpcHost;

            IRpcResponse result =
                registeredHost.Properties.ContainsKey(propertyName) ?
                new HostRpcResponse(registeredHost.Properties[propertyName].Guid) :
                new SimpleRpcResponse(host.GetPropertyValue(propertyName));

            var serialized = RpcJsonSerializer.Serialize(result);

            return Results.Ok(serialized);
        }

        public async Task<IResult> SetPropertyValue(Guid guid, string propertyName, HttpContext context)
        {
            var serializedValue = await this.ReadRawBody(context);
            var registeredHost = this.hostManager.GetRegisteredRpcHost(guid);
            var value = RpcJsonSerializer.DeserializeObject(serializedValue);

            registeredHost.RpcHost.SetPropertyValue(propertyName, value);

            return Results.Ok();
        }

        public async Task<IResult> InvokeMethod(Guid guid, string methodName, HttpContext context)
        {
            var registeredHost = this.hostManager.GetRegisteredRpcHost(guid);
            var serializedArguments = await this.ReadRawBody(context);

            var arguments = string.IsNullOrWhiteSpace(serializedArguments) ? new object[] { } : (object[])RpcJsonSerializer.DeserializeObject(serializedArguments);

            IRpcResponse result = new SimpleRpcResponse(registeredHost.RpcHost.ExecuteMethod(methodName, arguments));
            return Results.Ok(RpcJsonSerializer.Serialize(result));
        }

        private async Task<string> ReadRawBody(HttpContext context)
        {
            using var streamReader = new StreamReader(context.Request.Body);
            return await streamReader.ReadToEndAsync();
        }


    }
}
