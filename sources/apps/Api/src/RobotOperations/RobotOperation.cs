namespace Api.RobotOperations;

public static class RobotOperation
{
    public static DirectionEnum FindNewDirection(int xPrev, int yPrev, int xNext, int yNext)
    {
        if (xPrev - xNext == -1) return DirectionEnum.East;
        if (xPrev - xNext == 1) return DirectionEnum.West;
        if (yPrev - yNext == -1) return DirectionEnum.South;
        if (yPrev - yNext == 1) return DirectionEnum.North;

        throw new InvalidOperationException("Invalid move");
    }

    public static Position CalculatePosition(Position position, DirectionEnum oldDirection, RobotMoveEnum? move)
    {
        int deltaX = 0;
        int deltaY = 0;

        switch (move)
        {
            case RobotMoveEnum.Forward:
                switch (oldDirection)
                {
                    case DirectionEnum.North:
                        deltaY = -1;
                        break;
                    case DirectionEnum.East:
                        deltaX = 1;
                        break;
                    case DirectionEnum.South:
                        deltaY = 1;
                        break;
                    case DirectionEnum.West:
                        deltaX = -1;
                        break;
                }
                break;

            case RobotMoveEnum.Right:
                switch (oldDirection)
                {
                    case DirectionEnum.North:
                        deltaX = 1;
                        break;
                    case DirectionEnum.East:
                        deltaY = 1;
                        break;
                    case DirectionEnum.South:
                        deltaX = -1;
                        break;
                    case DirectionEnum.West:
                        deltaY = -1;
                        break;
                }
                break;

            case RobotMoveEnum.Back:
                switch (oldDirection)
                {
                    case DirectionEnum.North:
                        deltaY = 1;
                        break;
                    case DirectionEnum.East:
                        deltaX = -1;
                        break;
                    case DirectionEnum.South:
                        deltaY = -1;
                        break;
                    case DirectionEnum.West:
                        deltaX = 1;
                        break;
                }
                break;

            case RobotMoveEnum.Left:
                switch (oldDirection)
                {
                    case DirectionEnum.North:
                        deltaX = -1;
                        break;
                    case DirectionEnum.East:
                        deltaY = -1;
                        break;
                    case DirectionEnum.South:
                        deltaX = 1;
                        break;
                    case DirectionEnum.West:
                        deltaY = 1;
                        break;
                }
                break;
        }

        if (deltaX != 0 || deltaY != 0)
        {
            int xPrev = position.X;
            int yPrev = position.Y;

            int xNext = xPrev + deltaX;
            int yNext = yPrev + deltaY;

            return new Position() { X = xNext, Y = yNext };
        }

        throw new InvalidOperationException();
    }

    public static RobotMoveEnum GenerateMove(DirectionEnum oldDirection, DirectionEnum newDirection)
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