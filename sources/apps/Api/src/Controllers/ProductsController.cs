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
    public IActionResult BuyAndCollectProducts([FromBody] List<Product> products)
    {
        _logger.LogDebug("Handle api call to buy and collect products");

        Position startPosition = new Position() { X = 0, Y = -1 };

        List<Position> positions = new List<Position>() { startPosition };
        positions.AddRange(products.Select(product => product.Position).ToList());
        positions.Add(startPosition);

        List<Position> path = _algorithmProvider.GetAlgorithm().FindPath(positions);
        
        return Ok(path);
    }
}