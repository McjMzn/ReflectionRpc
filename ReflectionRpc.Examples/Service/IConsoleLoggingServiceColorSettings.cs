﻿namespace ReflectionRpc.Examples.Service
{
    public interface IConsoleLoggingServiceColorSettings
    {
        ConsoleColor BackgroundColor { get; set; }
        ConsoleColor ForegroundColor { get; set; }
        void ResetToDefault();
    }
}
