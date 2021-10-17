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
![image](https://i.imgur.com/BWEq1hZ.png)
## Client
```c#
IConsoleLoggingService consoleLoggingService = DynamicRpcClient.Create<IConsoleLoggingService>("http://localhost:5087/", "Console");
// At this point use it as regular local object.
```
## Interfaces used in example
```c#
public interface IConsoleLoggingService
{
    string MessagePrefix { get; set; }
    IConsoleLoggingServiceColorSettings ColorSettings { get; set; }
    int GetNumberOfLoggedMessages();
    void PrintConsoleMessage(string message);
    void PrintConsoleMessage(string message, int count);
}

public interface IConsoleLoggingServiceColorSettings
{
    ConsoleColor BackgroundColor { get; set; }
    ConsoleColor ForegroundColor { get; set; }
    void ResetToDefault();
}
```
