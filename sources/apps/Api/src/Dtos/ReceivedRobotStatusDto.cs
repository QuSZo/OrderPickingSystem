using Api.RobotOperations;

namespace Api.Dtos;

public record ReceivedRobotStatusDto
{
    public required RobotEventEnum Event { get; init; }
    public required long Timestamp { get; init; }
    public RobotCommand? Command { get; init; }
    public RobotPIDSummary? RobotPIDSummary { get; init; }
}