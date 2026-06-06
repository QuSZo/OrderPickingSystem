using Api.Dtos;
using Api.Logging;
using Api.Orders;

namespace Api.Services;

public class StatisticsService
{
    private readonly ILogger _logger;
    private readonly IOrdersRepository _ordersRepository;

    public StatisticsService(ILoggerFactory loggerFactory, IOrdersRepository ordersRepository)
    {
        _logger = loggerFactory.CreateLoggerApi();
        _ordersRepository = ordersRepository;
    }

    public async Task<List<AverageDuration>> GetAverageDuration()
    {
        _logger.LogInformation("Calculating average duration by algorithm");

        IReadOnlyList<OrderDto> orderDtos = await _ordersRepository.GetAllDtosAsync();

        List<AverageDuration> averageDurations = orderDtos
            .Where(order => order.FinishPickingTime != null && order.StartPickingTime != null)
            .Select(order => new
            {
                order.TspAlgorithm,
                Duration = order.FinishPickingTime!.Value - order.StartPickingTime!.Value
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

    public async Task<List<AverageDurationByOrderSize>> GetAverageDurationByOrderSizeAsync()
    {
        _logger.LogInformation("Calculating average duration by algorithm and order size");

        IReadOnlyList<OrderDto> orderDtos = await _ordersRepository.GetAllDtosAsync();
        
        List<AverageDurationByOrderSize> averageDurationsByOrderSize = orderDtos
            .Where(o => o.FinishPickingTime != null && o.StartPickingTime != null)
            .Select(o => new
            {
                o.TspAlgorithm,
                ProductCount = o.OrderedProducts.Count,
                Duration = o.FinishPickingTime!.Value - o.StartPickingTime!.Value
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