using Microsoft.Extensions.Logging;

namespace ApiClient
{
    public static class ConsoleLogger
    {
        public static ILogger Create()
        {
            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning)
                    .AddFilter("ApiClient", LogLevel.Debug)
                    .AddConsole();
            });
            ILogger logger = loggerFactory.CreateLogger<ApiClientService>();

            logger.LogInformation("Logger Initialized");

            return logger;
        }
    }
}
