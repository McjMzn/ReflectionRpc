using ImpromptuInterface;
using ReflectionRpc.Core;
using ReflectionRpc.Examples;
using ReflectionRpc.Examples.Service;


IConsoleLoggingService consoleLoggingService = DynamicRpcClient.Create<IConsoleLoggingService>("http://localhost:5087/", "Console");


Console.WriteLine($"[Get property value] Current message prefix is: {consoleLoggingService.MessagePrefix}");

Console.WriteLine($"[Void method invoke] Writing a message.");
consoleLoggingService.PrintConsoleMessage("This is a remotely logged message.");

Console.WriteLine($"[Set property value] Changing message prefix to: '>>'");
consoleLoggingService.MessagePrefix = ">>";

Console.WriteLine($"[Void method invoke] Writing multiple messages.");
consoleLoggingService.PrintConsoleMessage("This is one of messages logged in a batch.", 3);

Console.WriteLine($"[Set property value in nested client] Changing a foreground color.");
consoleLoggingService.Settings.ForegroundColor = ConsoleColor.Cyan;

Console.WriteLine($"[Void method invoke] Writing a message in color.");
consoleLoggingService.PrintConsoleMessage("This is a remotely logged message in color.");

Console.WriteLine($"[Int32 method invoke] In total {consoleLoggingService.GetNumberOfLoggedMessages()} have been logged.");

Console.WriteLine("[Invoke void method in nested client] Resetting color settings.");
consoleLoggingService.Settings.ResetToDefault();

Console.WriteLine($"[Void method invoke] Writing last message.");
consoleLoggingService.PrintConsoleMessage("No exception has been thrown. Everything works fine. Goodbye.");

Console.ReadKey();