using ReflectionRpc.Examples;
using ReflectionRpc.Examples.Rpc;
using ReflectionRpc.Examples.Service;

IConsoleLoggingService consoleLoggingService = new ConsoleLoggingServiceRpcClient("http://localhost:5087/", "Console Service");

consoleLoggingService.MessagePrefix = ">>";

var prefix = consoleLoggingService.MessagePrefix;

var settings = consoleLoggingService.Settings;

var color = settings.BackgroundColor;


consoleLoggingService.PrintConsoleMessage("dupa", 3);


var numberOfLogged = consoleLoggingService.GetNumberOfLoggedMessages();

consoleLoggingService.MessagePrefix = "INFO>";

settings.ForegroundColor = ConsoleColor.Green;

consoleLoggingService.PrintConsoleMessage($"Number of logged messages: {consoleLoggingService.GetNumberOfLoggedMessages()}");
    
Console.ReadKey();