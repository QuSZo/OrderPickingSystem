using Api.Logging;
using Api.Orders;

namespace Api.Services;

public class StatisticsService
{
    private readonly ILogger _logger;
    private readonly IOrdersRepository _historicalOrdersRepository;

    public StatisticsService(ILoggerFactory loggerFactory, IOrdersRepository historicalOrdersRepository)
    {
        _logger = loggerFactory.CreateLoggerApi();
        _historicalOrdersRepository = historicalOrdersRepository;
    }

    public List<AverageDuration> GetAverageDuration()
    {
        _logger.LogInformation("Calculating average duration by algorithm");

        List<AverageDuration> averageDurations = _historicalOrdersRepository.GetAll()
            .Where(order => order.FinishPickingTime != null)
            .Select(order => new
            {
                order.TspAlgorithm,
                Duration = order.FinishPickingTime!.Value - order.StartPickingTime
            })
            .GroupBy(order => order.TspAlgorithm)
            .Select(group => new AverageDuration
            {
                Algorithm = group.Key.ToString(),
                AverageDurationMs = group.Average(x => x.Duration)
            })
            .ToList();

        return averageDurations;
    }

    public List<AverageDurationByOrderSize> GetAverageDurationByOrderSize()
    {
        _logger.LogInformation("Calculating average duration by algorithm and order size");

        List<AverageDurationByOrderSize> averageDurationsByOrderSize = _historicalOrdersRepository.GetAll()
            .Where(o => o.FinishPickingTime != null)
            .Select(o => new
            {
                o.TspAlgorithm,
                ProductCount = o.OrderedProducts.Count,
                Duration = o.FinishPickingTime!.Value - o.StartPickingTime
            })
            .GroupBy(o => new { o.TspAlgorithm, o.ProductCount })
            .Select(g => new AverageDurationByOrderSize
            {
                Algorithm = g.Key.TspAlgorithm.ToString(),
                ProductCount = g.Key.ProductCount,
                AverageDurationMs = g.Average(x => x.Duration)
            })
            .OrderBy(x => x.ProductCount)
            .ToList();

        return averageDurationsByOrderSize;
    }
}