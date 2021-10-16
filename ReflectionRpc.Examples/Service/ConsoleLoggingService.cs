using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReflectionRpc.Examples.Service
{
    public class ConsoleLoggingService : IConsoleLoggingService
    {
        private int messageCounter = 0;

        public string MessagePrefix { get; set; } = string.Empty;
        public IConsoleLoggingServiceSettings Settings { get; set; } = new ConsoleLoggingServiceSettings();

        public int GetNumberOfLoggedMessages()
        {
            return messageCounter;
        }

        public void PrintConsoleMessage(string message)
        {
            Console.ForegroundColor = this.Settings.ForegroundColor;
            Console.BackgroundColor = this.Settings.BackgroundColor;

            Console.WriteLine($"{MessagePrefix}{message}");
            messageCounter++;

            Console.ResetColor();
        }

        public void PrintConsoleMessage(string message, int count)
        {
            for (var i = 0; i < count; i++)
            {
                PrintConsoleMessage(message);
            }
        }
    }
}
