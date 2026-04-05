using Api.RobotOperations;

namespace Api.Dtos;

public record RobotCommandDto
{
    public required List<RobotCommand> Commands { get; init; }
}