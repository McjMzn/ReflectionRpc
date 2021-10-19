using ReflectionRpc.Core;
using ReflectionRpc.Examples.Services;

IConsoleLoggingService consoleLoggingService = RpcProxy.Create<IConsoleLoggingService>("http://localhost:12345/", "Console");


Console.WriteLine($"[Get property value] Current message prefix is: {consoleLoggingService.MessagePrefix}");

Console.WriteLine($"[Void method invoke] Writing a message.");
consoleLoggingService.LogMessageToConsole("This is a remotely logged message.");

Console.WriteLine($"[Set property value] Changing message prefix to: '>>'");
consoleLoggingService.MessagePrefix = ">>";
Console.ReadKey();
Console.WriteLine($"[Void method invoke] Writing multiple messages.");
consoleLoggingService.LogMessageToConsole("This is one of messages logged in a batch.", 3);

Console.WriteLine($"[Set property value in nested client] Changing a foreground color.");
consoleLoggingService.ColorSettings.ForegroundColor = ConsoleColor.Cyan;

Console.WriteLine($"[Void method invoke] Writing a message in color.");
consoleLoggingService.LogMessageToConsole("This is a remotely logged message in color.");

Console.WriteLine($"[Int32 method invoke] In total {consoleLoggingService.GetNumberOfLoggedMessages()} have been logged.");

Console.WriteLine("[Invoke void method in nested client] Resetting color settings.");
consoleLoggingService.ColorSettings.ResetToDefault();

Console.WriteLine($"[Void method invoke] Writing last message.");
consoleLoggingService.LogMessageToConsole("No exception has been thrown. Everything works fine. Goodbye.");

Console.ReadKey();