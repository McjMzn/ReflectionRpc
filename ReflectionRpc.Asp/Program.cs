using ReflectionRpc.Asp;
using ReflectionRpc.Core;
using ReflectionRpc.Core.RpcResponses;
using ReflectionRpc.Examples.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRazorPages();

builder.Services.AddReflectionRpc();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapGet("/", () => Results.LocalRedirect("/swagger/index.html"));

app.UseReflectionRpc();
app.HostReflectionRpcService(new ConsoleLoggingService(), "Console");

app.MapRazorPages();

app.Run();
