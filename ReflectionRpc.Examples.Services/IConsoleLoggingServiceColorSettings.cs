namespace ReflectionRpc.Examples.Services
{
    public interface IConsoleLoggingServiceColorSettings
    {
        ConsoleColor BackgroundColor { get; set; }
        ConsoleColor ForegroundColor { get; set; }
        void ResetToDefault();
    }
}
