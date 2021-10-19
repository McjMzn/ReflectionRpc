namespace ReflectionRpc.Examples.Services
{
    public class ConsoleLoggingService : IConsoleLoggingService
    {
        private int messageCounter = 0;

        public string MessagePrefix { get; set; } = ">";

        public IConsoleLoggingServiceColorSettings ColorSettings { get; set; } = new ConsoleLoggingServiceColorSettings();

        public int GetNumberOfLoggedMessages()
        {
            return messageCounter;
        }

        public void LogMessageToConsole(string message)
        {
            Console.ForegroundColor = ColorSettings.ForegroundColor;
            Console.BackgroundColor = ColorSettings.BackgroundColor;

            Console.WriteLine($"{MessagePrefix}{message}");
            messageCounter++;

            Console.ResetColor();
        }

        public void LogMessageToConsole(string message, int count)
        {
            for (var i = 0; i < count; i++)
            {
                LogMessageToConsole(message);
            }
        }
    }
}
