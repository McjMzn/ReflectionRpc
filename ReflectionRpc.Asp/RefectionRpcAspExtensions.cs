using Microsoft.AspNetCore.Mvc;
using ReflectionRpc.Core;
using ReflectionRpc.Core.RpcResponses;
using ReflectionRpc.Core.Serialization;
using ReflectionRpc.Examples.Service;

namespace ReflectionRpc.Asp
{
    public static class RefectionRpcAspExtensions
    {
        public static IServiceCollection AddReflectionRpc(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IRpcHostManager, RpcHostManager>();
            return serviceCollection;
        }

        public static IApplicationBuilder HostReflectionRpcService(this IApplicationBuilder appBuilder, object service, string serviceTag = null)
        {
            appBuilder.ApplicationServices.GetService<IRpcHostManager>().RegisterHost(service, serviceTag);
            return appBuilder;
        }

        public static IApplicationBuilder UseReflectionRpc(this IApplicationBuilder appBuilder)
        {
            var routeRoot = "rpc";

            appBuilder.UseRouting().UseEndpoints(c =>
            {
                c.MapGet($"/{routeRoot}/hosts", GetHosts).Produces<List<RegisteredRpcHost>>();
                c.MapGet($"/{routeRoot}/hosts/tagged/{{tag}}", GetHostByTag).Produces<Guid>();
                c.MapGet($"/{routeRoot}/hosts/{{guid}}/properties/{{propertyName}}", GetPropertyValue).Produces<IRpcResponse>();
                c.MapPost($"/{routeRoot}/hosts/{{guid}}/properties/{{propertyName}}", SetPropertyValue);
                c.MapPost($"/{routeRoot}/hosts/{{guid}}/methods/{{methodName}}", InvokeMethod);
            });

            return appBuilder;
        }

        private static IResult GetHosts([FromServices] IRpcHostManager hostManager)
        {
            var hostInformations = hostManager.GetHosts();
            return Results.Ok(hostInformations);
        }

        private static IResult GetHostByTag([FromServices] IRpcHostManager hostManager, string tag)
        {
            var registeredHost = hostManager.GetHost(tag);
            return Results.Ok(registeredHost.Guid);
        }

        private static IResult GetPropertyValue([FromServices] IRpcHostManager hostManager, Guid guid, string propertyName)
        {
            var registeredHost = hostManager.GetHost(guid);
            var host = registeredHost.RpcHost;
            
            IRpcResponse result =
                registeredHost.Properties.ContainsKey(propertyName) ?
                new HostRpcResponse(registeredHost.Properties[propertyName].Guid) :
                new SimpleRpcResponse(host.GetPropertyValue(propertyName));

            var serialized = RpcJsonSerializer.Serialize(result);

            return Results.Ok(serialized);
        }

        private static async Task<IResult> SetPropertyValue([FromServices] IRpcHostManager hostManager, Guid guid, string propertyName, HttpContext context)
        {
            var serializedValue = await ReadRawBody(context);
            var registeredHost = hostManager.GetHost(guid);
            var value = RpcJsonSerializer.DeserializeObject(serializedValue);

            registeredHost.RpcHost.SetPropertyValue(propertyName, value);

            return Results.Ok();
        }

        private static async Task<IResult> InvokeMethod([FromServices] IRpcHostManager hostManager, Guid guid, string methodName, HttpContext context)
        {
            var registeredHost = hostManager.GetHost(guid);
            var serializedArguments = await ReadRawBody(context);

            var arguments =
                string.IsNullOrWhiteSpace(serializedArguments) ?
                new object[] { } :
                RpcJsonSerializer.DeserializeObject(serializedArguments) as object[];

            var returnedByMethod = registeredHost.RpcHost.ExecuteMethod(methodName, arguments);
            var result = new SimpleRpcResponse(returnedByMethod);
            return Results.Ok(RpcJsonSerializer.Serialize(result));
        }

        private static async Task<string> ReadRawBody(HttpContext context)
        {
            using var streamReader = new StreamReader(context.Request.Body);
            return await streamReader.ReadToEndAsync();
        }
    }
}
