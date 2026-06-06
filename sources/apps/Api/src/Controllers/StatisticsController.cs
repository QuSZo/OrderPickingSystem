using Api.Logging;
using Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/statistics")]
public class StatisticsController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly StatisticsService _statisticsService;

    public StatisticsController(ILoggerFactory loggerFactory, StatisticsService statisticsService)
    {
        _logger = loggerFactory.CreateLoggerApi();
        _statisticsService = statisticsService;
    }

    [HttpGet("average-duration-by-algorithm")]
    public async Task<ActionResult<string>> GetAverageDurationAsync()
    {
        _logger.LogInformation("Handle api call to get statistics about average duration by algorithm");
        
        List<AverageDuration> averageDurations = await _statisticsService.GetAverageDuration();

        return Ok(averageDurations);
    }

    [HttpGet("average-duration-by-algorithm-and-order-size")]
    public async Task<ActionResult<string>> GetAverageDurationByOrderSizeAsync()
    {
        _logger.LogInformation("Handle api call to get statistics about average duration by algorithm and order size");
        
        List<AverageDurationByOrderSize> averageDurations = await _statisticsService.GetAverageDurationByOrderSizeAsync();

        return Ok(averageDurations);
    }
}