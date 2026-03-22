using Api.Logging;
using Api.Products;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly ProductsRepository _productsService;

    public ProductsController(
        ILoggerFactory loggerFactory, 
        ProductsRepository productsService)
    {
        _logger = loggerFactory.CreateLoggerApi();
        _productsService = productsService;
    }

    [HttpGet]
    public ActionResult<string> GetProducts()
    {
        _logger.LogDebug("Handle api call to get all products");
        IReadOnlyList<Product> products = _productsService.GetProducts();

        return Ok(products);
    }
}