using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

namespace MessagingCorp.Common.Logger
{
    public static class Loggerino
    {
        public static LoggerConfiguration ToConsole(this LoggerConfiguration loggerConfig)
        {
            return loggerConfig.WriteTo.Console();
        }

        public static LoggerConfiguration ToFile(this LoggerConfiguration loggerConfig, string logFilePath)
        {
            return loggerConfig.WriteTo.File(
                new RenderedCompactJsonFormatter(),
                logFilePath,
                rollingInterval: RollingInterval.Day);
        }

        public static LoggerConfiguration ToConsoleAndFile(this LoggerConfiguration loggerConfig, string logFilePath)
        {
            return loggerConfig.WriteTo.File(
                    new RenderedCompactJsonFormatter(),
                    logFilePath,
                    rollingInterval: RollingInterval.Day)
                .WriteTo.Console();
        }

        public static LoggerConfiguration WithLogLevel(this LoggerConfiguration loggerConfig, LogEventLevel logLevel)
        {
            return loggerConfig.MinimumLevel.Is(logLevel);
        }

        public static ILogger ForContextWithConfig<T>(
            this ILogger logger,
            string? logFilePath = null,
            bool fileAndConsole = false,
            LogEventLevel logLevel = LogEventLevel.Information)
        {
            var loggerConfig = new LoggerConfiguration();

            if (fileAndConsole && !string.IsNullOrEmpty(logFilePath))
                loggerConfig = loggerConfig.ToConsoleAndFile(logFilePath);
            else
                loggerConfig = string.IsNullOrEmpty(logFilePath) ? loggerConfig.ToConsole() : loggerConfig.ToFile(logFilePath);
            
            loggerConfig = loggerConfig.WithLogLevel(logLevel);

            return loggerConfig.CreateLogger().ForContext<T>();
        }
    }
}
