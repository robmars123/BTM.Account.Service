namespace BTM.Account.Application.Abstractions
{
    public interface ILoggingService
    {
        void LogError(string message, Exception exception);
        void LogError(string message);
    }
}
