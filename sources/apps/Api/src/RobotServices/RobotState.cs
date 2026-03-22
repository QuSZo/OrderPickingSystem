using Api.Orders;
using Api.RobotOperations;

namespace Api.RobotServices;

public class RobotState
{
    public Position Position { get; set; } = new Position() { X = 0, Y = 0 };
    public DirectionEnum Direction { get; set; } = DirectionEnum.South;
    public RobotMoveEnum? Command { get; set; }
    public RobotEventEnum? Event { get; set; }
    public double? Timestamp { get; set; }
    public Order? Order { get; set; }

    public void StartPicking(Order order)
    {
        Order = order;
    }

    public void Update(Position position, DirectionEnum direction, RobotMoveEnum? move, RobotEventEnum currEvent, double timestamp)
    {
        Position = position;
        Direction = direction;
        Command = move;
        Event = currEvent;
        Timestamp = timestamp;
    }

    public void Reset()
    {
        Position = new Position() { X = 0, Y = 0 };
        Direction = DirectionEnum.South;
        Command = null;
        Event = null;
        Timestamp = null;
        Order = null;
    }
}