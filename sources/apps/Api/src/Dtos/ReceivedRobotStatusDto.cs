using Api.RobotOperations;

namespace Api.Dtos;

public record ReceivedRobotStatusDto
{
    public required RobotEventEnum Event { get; init; }
    public required double Timestamp { get; init; }
    public RobotCommand? Command { get; init; }
}