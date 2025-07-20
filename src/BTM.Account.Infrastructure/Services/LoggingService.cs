using BTM.Account.Application.Abstractions;
using Serilog;

namespace BTM.Account.Infrastructure.Services
{
    public class LoggingService : ILoggingService
    {
        private readonly ILogger _logger;
        public LoggingService()
        {
            _logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("logs/exception.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();
        }
        public void LogError(string message, Exception exception)
        {
            _logger.Error(exception, message);
        }

        public void LogError(string message)
        {
            _logger.Error(message);
        }
    }
}
