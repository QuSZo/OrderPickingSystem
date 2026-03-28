using Api.Dtos;
using Api.Logging;
using Api.RobotServices;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/robot")]
public class RobotController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly RobotInbound _robotInbound;
    private readonly RobotState _robotState;

    public RobotController(RobotInbound robotInbound, ILoggerFactory loggerFactory, RobotState robotState)
    {
        _logger = loggerFactory.CreateLoggerApi();
        _robotInbound = robotInbound;
        _robotState = robotState;
    }

    [HttpGet("state")]
    public ActionResult<string> GetRobotState()
    {
        _logger.LogInformation("Handle api call to get current robot state");

        return Ok(_robotState);
    }

    [HttpPost("command")]
    public async Task<ActionResult<string>> RobotCommand([FromBody] RobotCommandDto commands)
    {
        _logger.LogInformation("Handle api call with new robot commands");
        await _robotInbound.SendCommands(commands);
        
        return Ok();
    }

    [HttpPost("stop")]
    public async Task<ActionResult<string>> RobotStop()
    {
        _logger.LogInformation("Handle api call to stop the robot");
        await _robotInbound.SendStop();

        return Ok();
    }
}