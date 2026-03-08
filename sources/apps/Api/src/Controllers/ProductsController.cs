using Api.Logging;
using Api.Products;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly ProductsService _productsService;

    public ProductsController(ILoggerFactory loggerFactory, ProductsService productsService)
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