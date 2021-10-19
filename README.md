# ReflectionRpc
Project started out of curiosity if it would be possible to use reflection for operating on remote objects just as they were local ones in such way that no additional wrappers or contract definitions would be required to use it.

I consider this proof of concept to be successful even though it has some limitations and requires additional polish. 

## Server
![image](https://i.imgur.com/zrTLjd4.png)
### Extending existing web application
```c#
var builder = WebApplication.CreateBuilder(args);
[...]
builder.Services.AddReflectionRpc();
[...]
var app = builder.Build();
[...]
app.UseReflectionRpc();
app.RegisterAsRpcHost(new ConsoleLoggingService(), "Console");
[...]
app.Run();
```
### Using `ReflectionRpcWebServer` class
```c#
var server = new ReflectionRpcWebServer();
server.RegisterAsRpcHost(new ConsoleLoggingService(), "Console");
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
    void LogMessageToConsole(string message);
    void LogMessageToConsole(string message, int count);
}

public interface IConsoleLoggingServiceColorSettings
{
    ConsoleColor BackgroundColor { get; set; }
    ConsoleColor ForegroundColor { get; set; }
    void ResetToDefault();
}
```
