using Api.Logging;
using Api.TravelingSalesmanAlgorithms.BruteForceAlgorithms;
using Api.TravelingSalesmanAlgorithms.AStarGreedyAlgorithms;
using Api.TravelingSalesmanAlgorithms.NaiveAlgorithms;
using Api.TravelingSalesmanAlgorithms.DijkstraGreedyAlgorithms;
using Api.TravelingSalesmanAlgorithms.NetworkxAlgorithms;

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
            case TspAlgorithmsEnum.DijkstraZachłanny:
                return new DijkstraGreedyAlgorithm(_loggerFactory);
            case TspAlgorithmsEnum.AStarZachłanny:
                return new AStarGreedyAlgorithm(_loggerFactory);
            case TspAlgorithmsEnum.Networkx:
                return new NetworkxAlgorithm(_loggerFactory);
            case TspAlgorithmsEnum.Naiwny:
                return new NaiveAlgorithm(_loggerFactory);
            case TspAlgorithmsEnum.BrutalnaSiła:
                return new BruteForceAlgorithm(_loggerFactory);
            default:
                return new AStarGreedyAlgorithm(_loggerFactory);
        }
    }
}