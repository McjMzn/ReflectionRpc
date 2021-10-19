namespace ReflectionRpc.Examples.Services
{
    public interface IConsoleLoggingService
    {
        string MessagePrefix { get; set; }
        IConsoleLoggingServiceColorSettings ColorSettings { get; set; }
        int GetNumberOfLoggedMessages();
        void LogMessageToConsole(string message);
        void LogMessageToConsole(string message, int count);
    }
}
