using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReflectionRpc.Examples.Service
{
    [Serializable]
    public class ConsoleLoggingService : IConsoleLoggingService
    {
        private int messageCounter = 0;

        public string MessagePrefix { get; set; } = ">";
       
        public IConsoleLoggingServiceColorSettings ColorSettings { get; set; } = new ConsoleLoggingServiceColorSettings();

        public int GetNumberOfLoggedMessages()
        {
            return messageCounter;
        }

        public void PrintConsoleMessage(string message)
        {
            Console.ForegroundColor = this.ColorSettings.ForegroundColor;
            Console.BackgroundColor = this.ColorSettings.BackgroundColor;

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
