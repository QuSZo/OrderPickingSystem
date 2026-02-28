namespace Api.Logging;

public static class LoggerFactoryExtension
{
    public static ILogger CreateLoggerApi(this ILoggerFactory loggerFactory)
    {
        return loggerFactory.CreateLogger("api");
    }
}