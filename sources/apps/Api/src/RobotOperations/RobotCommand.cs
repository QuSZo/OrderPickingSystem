using Api.Orders;

namespace Api.RobotOperations;

public record RobotCommand
{
    public RobotMoveEnum Move { get; init; }
    public int? StopDurationMs { get; init; }
    public OrderedProduct? OrderedProduct { get; init; }
    public double? Distance {get; init; }
}