using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReflectionRpc.Examples.Service
{
    public interface IConsoleLoggingServiceColorSettings
    {
        ConsoleColor BackgroundColor { get; set; }
        ConsoleColor ForegroundColor { get; set; }
        void ResetToDefault();
    }
}
