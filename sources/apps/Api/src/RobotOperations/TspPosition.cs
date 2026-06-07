using Microsoft.EntityFrameworkCore;

namespace Api.RobotOperations;

public record TspPosition
{
    public required Guid Id { get; init; }
    public required int X { get; init; }
    public required int Y { get; init; }
    public required int OrderNumber { get; init; }
}