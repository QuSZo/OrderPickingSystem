using Api.Logging;
using Api.Products;

namespace Api.RobotOperations;

public class RobotOperation
{
    private readonly ILogger _logger; 

    public RobotOperation(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLoggerApi();
    }

    public List<RobotMoveEnum> GenerateMoves(List<Position> positions, DirectionEnum startDirection)
    {
        _logger.LogDebug("Generating robot moves from positions");

        List<RobotMoveEnum> moves = new List<RobotMoveEnum>();

        DirectionEnum oldDirection = startDirection;

        for (int i = 0; i < positions.Count - 1; i++)
        {
            int x_prev = positions[i].X;
            int y_prev = positions[i].Y;

            int x_next = positions[i+1].X;
            int y_next = positions[i+1].Y;

            DirectionEnum newDirection = FindNewDirection(x_prev, y_prev, x_next, y_next);
            RobotMoveEnum newMove = GenerateMove(oldDirection, newDirection);

            moves.Add(newMove);

            oldDirection = newDirection;
        }

        _logger.LogDebug($"Generated moves: { string.Join(" -> ", moves) }");
        return moves;
    }

    private DirectionEnum FindNewDirection(int x_prev, int y_prev, int x_next, int y_next)
    {
        if (x_prev - x_next == -1) return DirectionEnum.East;
        if (x_prev - x_next == 1) return DirectionEnum.West;
        if (y_prev - y_next == -1) return DirectionEnum.South;
        if (y_prev - y_next == 1) return DirectionEnum.North;

        throw new InvalidOperationException("Invalid move");
    }

    private RobotMoveEnum GenerateMove(DirectionEnum oldDirection, DirectionEnum newDirection)
    {
        switch (newDirection)
        {
            case DirectionEnum.North:
                switch (oldDirection)
                {
                    case DirectionEnum.North:
                        return RobotMoveEnum.Forward;
                    case DirectionEnum.East:
                        return RobotMoveEnum.Left;
                    case DirectionEnum.South:
                        return RobotMoveEnum.Back;
                    case DirectionEnum.West:
                        return RobotMoveEnum.Right;
                }
                break;

            case DirectionEnum.East:
                switch (oldDirection)
                {
                    case DirectionEnum.North:
                        return RobotMoveEnum.Right;
                    case DirectionEnum.East:
                        return RobotMoveEnum.Forward;
                    case DirectionEnum.South:
                        return RobotMoveEnum.Left;
                    case DirectionEnum.West:
                        return RobotMoveEnum.Back;
                }
                break;

            case DirectionEnum.South:
                switch (oldDirection)
                {
                    case DirectionEnum.North:
                        return RobotMoveEnum.Back;
                    case DirectionEnum.East:
                        return RobotMoveEnum.Right;
                    case DirectionEnum.South:
                        return RobotMoveEnum.Forward;
                    case DirectionEnum.West:
                        return RobotMoveEnum.Left;
                }
                break;

            case DirectionEnum.West:
                switch (oldDirection)
                {
                    case DirectionEnum.North:
                        return RobotMoveEnum.Left;
                    case DirectionEnum.East:
                        return RobotMoveEnum.Back;
                    case DirectionEnum.South:
                        return RobotMoveEnum.Right;
                    case DirectionEnum.West:
                        return RobotMoveEnum.Forward;
                }
                break;
        }

        throw new InvalidOperationException();
    }
}