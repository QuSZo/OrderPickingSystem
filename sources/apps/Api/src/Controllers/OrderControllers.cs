using Api.Logging;
using Api.Orders;
using Api.RobotService;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/orders")]
public class OrderControllers : ControllerBase
{
    private readonly ILogger _logger;
    private readonly RobotInbound _robotInbound;
    private readonly HistoricalOrdersRepository _historicalOrdersRepository;

    public OrderControllers(
        ILoggerFactory loggerFactory, 
        RobotInbound robotInbound,
        HistoricalOrdersRepository historicalOrdersRepository)
    {
        _logger = loggerFactory.CreateLoggerApi();
        _robotInbound = robotInbound;
        _historicalOrdersRepository = historicalOrdersRepository;
    }

    [HttpGet]
    public ActionResult<string> GetHistoricalOrders()
    {
        _logger.LogDebug("Handle api call to get all historical orders");
        IReadOnlyList<Order> orders = _historicalOrdersRepository.GetAll();

        return Ok(orders);
    }

    [HttpPost("buy")]
    public async Task<IActionResult> BuyAndCollectProducts([FromBody] Order order)
    {
        _logger.LogDebug("Handle api call to buy and collect products");

        _historicalOrdersRepository.Add(order);
        await _robotInbound.StartPicking(order);
        
        return Ok();
    }
}
