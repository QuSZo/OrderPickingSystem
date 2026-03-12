using Api.Logging;
using Api.Products;
using Api.TravelingSalesmanAlgorithms;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly ProductsService _productsService;
    private readonly TravelingSalesmanAlgorithmProvider _algorithmProvider;

    public ProductsController(
        ILoggerFactory loggerFactory, 
        ProductsService productsService, 
        TravelingSalesmanAlgorithmProvider algorithmProvider)
    {
        _logger = loggerFactory.CreateLoggerApi();
        _productsService = productsService;
        _algorithmProvider = algorithmProvider;
    }

    [HttpGet]
    public ActionResult<string> GetProducts()
    {
        _logger.LogDebug("Handle api call to get all products");
        IReadOnlyList<Product> products = _productsService.GetProducts();

        return Ok(products);
    }

    [HttpPost("buy")]
    public IActionResult BuyAndCollectProducts()
    {
        _logger.LogDebug("Handle api call to buy and collect products");

        // temp
        List<Position> positions = new List<Position>() { new Position() { X = 0, Y = 0 }, new Position() { X = 0, Y = 1 } };
        
        _algorithmProvider.GetAlgorithm().FindPath(positions);
        
        return Ok();
    }
}