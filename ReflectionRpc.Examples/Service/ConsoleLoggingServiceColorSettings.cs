using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReflectionRpc.Examples.Service
{
    public class ConsoleLoggingServiceColorSettings : IConsoleLoggingServiceColorSettings
    {
        public ConsoleLoggingServiceColorSettings()
        {
            this.BackgroundColor = Console.BackgroundColor;
            this.ForegroundColor = Console.ForegroundColor;
        }

        public ConsoleColor BackgroundColor { get; set; }
        
        public ConsoleColor ForegroundColor { get; set; }
        
        public void ResetToDefault()
        {
            this.BackgroundColor = ConsoleColor.Black;
            this.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
