using ReflectionRpc.Examples.Services;
using ReflectionRpc.WebServer;

var rpcServer = new ReflectionRpcWebServer();

rpcServer.RegisterAsRpcHost(new ConsoleLoggingService(), "Console");

rpcServer.Run("http://localhost:12345");