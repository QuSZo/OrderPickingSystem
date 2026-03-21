using Api.RobotOperations;

namespace Api.Dtos;

public record RobotCommandDto
{
    public required List<RobotMoveEnum> Commands { get; init; }
}