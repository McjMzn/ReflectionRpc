using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReflectionRpc.Examples.Service
{
    public interface IConsoleLoggingService
    {
        string MessagePrefix { get; set; }
        IConsoleLoggingServiceSettings Settings { get; set; }

        int GetNumberOfLoggedMessages();
        void PrintConsoleMessage(string message);
        void PrintConsoleMessage(string message, int count);
    }
}
