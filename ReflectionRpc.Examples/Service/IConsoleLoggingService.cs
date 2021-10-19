﻿namespace ReflectionRpc.Examples.Service
{
    public interface IConsoleLoggingService
    {
        string MessagePrefix { get; set; }
        IConsoleLoggingServiceColorSettings ColorSettings { get; set; }
        int GetNumberOfLoggedMessages();
        void PrintConsoleMessage(string message);
        void PrintConsoleMessage(string message, int count);
    }
}
