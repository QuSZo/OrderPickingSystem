using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/home")]
public class HomeController : ControllerBase
{
    public HomeController()
    {
    }

    [HttpGet]
    public ActionResult<string> GetMessage()
    {
        return Ok("Hello world");
    }
}