using Api.Orders;
using Api.RobotOperations;

namespace Api.RobotServices;

public class RobotState
{
    public Position CurrentPosition { get; set; } = new Position() { X = 0, Y = 0 };
    public Position? NextPosition { get; set; }
    public DirectionEnum Direction { get; set; } = DirectionEnum.South;
    public RobotCommand? Command { get; set; }
    public RobotEventEnum? Event { get; set; }
    public double? Timestamp { get; set; }
    public Order? Order { get; set; }
    public OrderedProduct? ProductBeingPicked { get; set; }

    public void StartPicking(Order order)
    {
        Order = order;
    }

    public void Reset()
    {
        CurrentPosition = new Position() { X = 0, Y = 0 };
        NextPosition = null;
        Direction = DirectionEnum.South;
        Command = null;
        Event = null;
        Timestamp = null;
        Order = null;
        ProductBeingPicked = null;
    }
}