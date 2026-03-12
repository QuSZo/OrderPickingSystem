using Api.Logging;

namespace Api.TravelingSalesmanAlgorithms;

public class TravelingSalesmanAlgorithmProvider
{
    private readonly ILogger _logger;
    private readonly ILoggerFactory _loggerFactory;
    private string _algorithmType;

    public TravelingSalesmanAlgorithmProvider(ILoggerFactory loggerFactory)
    {
        _loggerFactory = loggerFactory;
        _logger = loggerFactory.CreateLoggerApi();
        _algorithmType = "naive";
    }

    public void ChangeAlgorithm(string algorithmType)
    {
        _algorithmType = algorithmType;
        _logger.LogInformation($"Changed algorithm type to: {_algorithmType}");
    }

    public ITravelingSalesmanAlgorithm GetAlgorithm()
    {
        switch (_algorithmType)
        {
            case "naive":
                return new NaiveAlgorithm(_loggerFactory);
            default:
                return new NaiveAlgorithm(_loggerFactory);
        }
    }
}