using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ReflectionRpc.Asp;
using ReflectionRpc.Core;
using ReflectionRpc.Core.RpcResponses;
using ReflectionRpc.Examples.Service;
using RestSharp;
using System.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();


var manager = new RpcHostManager();
manager.RegisterHost(new ConsoleLoggingService(), "Console Service");

var rpcController = new RpcController(manager);


app.MapGet("/rpc/hosts", rpcController.GetHosts).Produces<List<RegisteredRpcHost>>();

app.MapGet("/rpc/hosts/tagged/{tag}", rpcController.GetHostByTag).Produces<Guid>();

app.MapGet("/rpc/hosts/{guid}/properties/{propertyName}", rpcController.GetPropertyValue).Produces<IRpcResponse>();

app.MapPost("/rpc/hosts/{guid}/properties/{propertyName}", rpcController.SetPropertyValue);

app.MapPost("/rpc/hosts/{guid}/methods/{methodName}", rpcController.InvokeMethod);

app.MapGet("/", () => Results.LocalRedirect("/swagger/index.html"));

app.Run();
