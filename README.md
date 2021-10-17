# ReflectionRpc
Project started out of curiosity if it would be possible to use reflection for operating on remote objects just as they were local ones in such way that no additional wrappers or contract definitions would be required to use it.

I consider this proof of concept to be really successful (but also requiring some polish). 

## Server
![image](https://i.imgur.com/zrTLjd4.png)
### Extend existing web application
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
### Use `ReflectionRpcWebServer` class
```c#
var server = new ReflectionRpcWebServer();
server.AddHostedService(new ConsoleLoggingService(), "Console");
server.Run("http://localhost:12345");
```
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
