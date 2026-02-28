using System.Text.Json;
using Api.Dtos;
using Api.Logging;
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
    public async Task<ActionResult<string>> RobotCommand([FromBody] RobotCommandDto dto)
    {
        _logger.LogDebug("Handle api call with new robot commands");
        string message = JsonSerializer.Serialize(dto);
        await _robotInbound.HandleCommand(message);
        
        return Ok();
    }

    [HttpPost("stop")]
    public async Task<ActionResult<string>> RobotStop()
    {
        _logger.LogDebug("Handle api call to stop the robot");
        await _robotInbound.HandleStop();

        return Ok();
    }
}