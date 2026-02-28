using Api.Logging;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/home")]
public class HomeController : ControllerBase
{
    private readonly ILogger _logger;

    public HomeController(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLoggerApi();
    }

    [HttpGet]
    public ActionResult<string> GetMessage()
    {
        _logger.LogDebug("Handle api call to get hello world message");
        return Ok("Hello world");
    }
}