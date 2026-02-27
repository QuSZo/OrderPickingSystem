using System.Text.Json;
using Api.Dtos;
using Api.RobotService;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/robot")]
public class RobotController : ControllerBase
{
    private readonly RobotInbound _robotInbound;

    public RobotController(RobotInbound robotInbound)
    {
        _robotInbound = robotInbound;
    }

    [HttpPost("command")]
    public async Task<ActionResult<string>> RobotCommand([FromBody] RobotCommandDto dto)
    {
        string message = JsonSerializer.Serialize(dto);
        await _robotInbound.HandleCommand(message);
        return Ok();
    }

    [HttpPost("stop")]
    public async Task<ActionResult<string>> RobotStop()
    {
        await _robotInbound.HandleStop();
        return Ok();
    }
}