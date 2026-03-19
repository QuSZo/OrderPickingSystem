using Api.Dtos;
using Api.Logging;
using Api.Products;
using Api.RobotService;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly ProductsService _productsService;
    private readonly RobotInbound _robotInbound;

    public ProductsController(
        ILoggerFactory loggerFactory, 
        ProductsService productsService, 
        RobotInbound robotInbound)
    {
        _logger = loggerFactory.CreateLoggerApi();
        _productsService = productsService;
        _robotInbound = robotInbound;
    }

    [HttpGet]
    public ActionResult<string> GetProducts()
    {
        _logger.LogDebug("Handle api call to get all products");
        IReadOnlyList<Product> products = _productsService.GetProducts();

        return Ok(products);
    }

    [HttpPost("buy")]
    public async Task<IActionResult> BuyAndCollectProducts([FromBody] List<OrderedProductDto> products)
    {
        _logger.LogDebug("Handle api call to buy and collect products");

        await _robotInbound.StartPicking(products);
        
        return Ok();
    }
}