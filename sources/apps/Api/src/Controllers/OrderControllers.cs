using Api.Commands;
using Api.Dtos;
using Api.Logging;
using Api.Orders;
using Api.RobotServices;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/orders")]
public class OrderControllers : ControllerBase
{
    private readonly ILogger _logger;
    private readonly RobotInbound _robotInbound;
    private readonly IOrdersRepository _ordersRepository;

    public OrderControllers(
        ILoggerFactory loggerFactory, 
        RobotInbound robotInbound,
        IOrdersRepository ordersRepository)
    {
        _logger = loggerFactory.CreateLoggerApi();
        _robotInbound = robotInbound;
        _ordersRepository = ordersRepository;
    }

    [HttpGet]
    public async Task<ActionResult<string>> GetOrders()
    {
        _logger.LogInformation("Handle api call to get all historical orders");
        IReadOnlyList<OrderDto> orders = await _ordersRepository.GetAllDtosAsync();

        return Ok(orders);
    }

    [HttpPost("buy")]
    public async Task<IActionResult> BuyAndCollectProducts([FromBody] CreateOrderCommand createOrderCommand)
    {
        _logger.LogInformation("Handle api call to buy and collect products");

        try
        {
            await _robotInbound.StartPicking(createOrderCommand);
        }
        catch (Exception excepion)
        {
            return BadRequest(excepion.Message);
        }
        
        return Ok();
    }

    [HttpPost("order-again")]
    public async Task<IActionResult> OrderAgain([FromBody] OrderAgainCommand orderAgainCommand)
    {
        _logger.LogInformation("Handle api call to order again");

        try
        {
            await _robotInbound.OrderAgain(orderAgainCommand);
        }
        catch (Exception excepion)
        {
            return BadRequest(excepion.Message);
        }
        
        return Ok();
    }
}
