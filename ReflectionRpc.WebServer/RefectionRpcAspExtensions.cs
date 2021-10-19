using Microsoft.AspNetCore.Mvc;
using ReflectionRpc.Core;
using ReflectionRpc.Core.Communication;
using ReflectionRpc.Core.Communication.RpcRequests;
using ReflectionRpc.Core.Communication.RpcResponses;
using ReflectionRpc.Core.Communication.Serialization;
using ReflectionRpc.Core.Interfaces;

namespace ReflectionRpc.WebServer
{
    public static class RefectionRpcAspExtensions
    {
        public static IServiceCollection AddReflectionRpc(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IRpcHostManager, RpcHostManager>();
            return serviceCollection;
        }

        public static IApplicationBuilder RegisterAsRpcHost(this IApplicationBuilder appBuilder, object service, string serviceTag = null)
        {
            appBuilder.ApplicationServices.GetService<IRpcHostManager>().RegisterAsRpcHost(service, serviceTag);
            return appBuilder;
        }

        public static IApplicationBuilder UseReflectionRpc(this IApplicationBuilder appBuilder)
        {
            var routeBase = "rpc";

            appBuilder.UseRouting().UseEndpoints((Action<IEndpointRouteBuilder>)(c =>
            {
                c.MapGet($"/{routeBase}/hosts", GetHosts).Produces<List<RegisteredRpcHost>>();
                c.MapGet($"/{routeBase}/hosts/tagged/{{tag}}", GetHostByTag).Produces<Guid>();
                c.MapGet($"/{routeBase}/hosts/{{guid}}/properties/{{propertyName}}", GetPropertyValue).Produces<IRpcResponse>();
                c.MapPost($"/{routeBase}/hosts/{{guid}}/properties/{{propertyName}}", SetPropertyValue);
                c.MapPost($"/{routeBase}/hosts/{{guid}}/methods/{{methodName}}", ExecuteMethod);
            }));

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
            var result = hostManager.ExecuteRequest(guid, new GetPropertyValueRequest(propertyName));

            return Results.Ok(RpcJsonSerializer.Serialize(result));
        }

        private static async Task<IResult> SetPropertyValue([FromServices] IRpcHostManager hostManager, Guid guid, string propertyName, HttpContext context)
        {
            var serializedValue = await ReadRawBody(context);
            var request = new SetPropertyValueRequest(propertyName, RpcJsonSerializer.DeserializeObject(serializedValue));

            var result = hostManager.ExecuteRequest(guid, request);
            return Results.Ok(RpcJsonSerializer.Serialize(result));
        }

        private static async Task<IResult> ExecuteMethod([FromServices] IRpcHostManager hostManager, Guid guid, string methodName, HttpContext context)
        {
            var serializedArguments = await ReadRawBody(context);
            var arguments =
                string.IsNullOrWhiteSpace(serializedArguments) ?
                new object[] { } :
                RpcJsonSerializer.DeserializeObject(serializedArguments) as object[];

            var result = hostManager.ExecuteRequest(guid, new ExecuteMethodRequest(methodName, arguments));

            return Results.Ok(RpcJsonSerializer.Serialize(result));
        }

        private static async Task<string> ReadRawBody(HttpContext context)
        {
            using var streamReader = new StreamReader(context.Request.Body);
            return await streamReader.ReadToEndAsync();
        }
    }
}
