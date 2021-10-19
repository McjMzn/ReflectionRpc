namespace ReflectionRpc.Examples.Services
{
    public class ConsoleLoggingServiceColorSettings : IConsoleLoggingServiceColorSettings
    {
        public ConsoleLoggingServiceColorSettings()
        {
            BackgroundColor = Console.BackgroundColor;
            ForegroundColor = Console.ForegroundColor;
        }

        public ConsoleColor BackgroundColor { get; set; }

        public ConsoleColor ForegroundColor { get; set; }

        public void ResetToDefault()
        {
            BackgroundColor = ConsoleColor.Black;
            ForegroundColor = ConsoleColor.Gray;
        }
    }
}
