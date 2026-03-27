using Api.Logging;

namespace Api.TravelingSalesmanAlgorithms;

public class TravelingSalesmanAlgorithmProvider
{
    private readonly ILogger _logger;
    private readonly ILoggerFactory _loggerFactory;

    public TravelingSalesmanAlgorithmProvider(ILoggerFactory loggerFactory)
    {
        _loggerFactory = loggerFactory;
        _logger = loggerFactory.CreateLoggerApi();
    }

    public ITravelingSalesmanAlgorithm GetAlgorithm(TspAlgorithmsEnum tspAlgorithm)
    {
        switch (tspAlgorithm)
        {
            case TspAlgorithmsEnum.Zachłanny:
                return new NaiveAlgorithm(_loggerFactory);
            case TspAlgorithmsEnum.BrutalnaSiła:
                return new BruteForceAlgorithm(_loggerFactory);
            default:
                return new NaiveAlgorithm(_loggerFactory);
        }
    }
}