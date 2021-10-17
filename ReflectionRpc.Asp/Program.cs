using ReflectionRpc.Asp;
using ReflectionRpc.Examples.Service;

var server = new ReflectionRpcWebServer();

server.AddHostedService(new ConsoleLoggingService(), "Console");

server.Run("http://localhost:12350");