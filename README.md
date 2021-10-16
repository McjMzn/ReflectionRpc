# ReflectionRpc
A simple HTTP based RPC relying on reflection. Project started out of curiosity if that can even be achieved. Got to admit, that it kinda works :D

## Server
```c#
var builder = WebApplication.CreateBuilder(args);
[...]
builder.Services.AddReflectionRpc();
[...]
var app = builder.Build();
[...]
app.UseReflectionRpc();
app.HostReflectionRpcService(new ConsoleLoggingService(), "Console");
[...]
app.Run();
```
![image](https://i.imgur.com/RvWLusS.png)
## Client
```c#
IConsoleLoggingService consoleLoggingService = DynamicRpcClient.Create<IConsoleLoggingService>("http://localhost:5087/", "Console");
```
