using Api.Dtos;
using Api.Logging;
using Api.RobotOperations;
using Api.RobotService;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/robot")]
public class RobotController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly RobotInbound _robotInbound;

    public RobotController(RobotInbound robotInbound, ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLoggerApi();
        _robotInbound = robotInbound;
    }

    [HttpPost("command")]
    public async Task<ActionResult<string>> RobotCommand([FromBody] RobotCommandDto commands)
    {
        _logger.LogDebug("Handle api call with new robot commands");
        await _robotInbound.SendCommands(commands);
        
        return Ok();
    }

    [HttpPost("stop")]
    public async Task<ActionResult<string>> RobotStop()
    {
        _logger.LogDebug("Handle api call to stop the robot");
        await _robotInbound.SendStop();

        return Ok();
    }
}