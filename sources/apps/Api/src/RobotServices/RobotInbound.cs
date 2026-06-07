using Api.Commands;
using Api.Dtos;
using Api.Logging;
using Api.Mqtt;
using Api.Orders;
using Api.RobotOperations;
using Api.TravelingSalesmanAlgorithms;
using Api.Utils;

namespace Api.RobotServices;

public class RobotInbound
{
    private const int StopDurationMsForOne = 1000;
    private readonly ILogger _logger;
    private readonly MqttProducer _mqttProducer;
    private readonly TravelingSalesmanAlgorithmProvider _algorithmProvider;
    private readonly RobotState _robotState;
    private readonly IOrdersRepository _ordersRepository;

    public RobotInbound(
        ILoggerFactory loggerFactory, 
        MqttProducer mqttProducer, 
        TravelingSalesmanAlgorithmProvider algorithmProvider,
        RobotState robotState,
        IOrdersRepository ordersRepository)
    {
        _logger = loggerFactory.CreateLoggerApi();
        _mqttProducer = mqttProducer;
        _algorithmProvider = algorithmProvider;
        _robotState = robotState;
        _ordersRepository = ordersRepository;
    }

    public async Task SendRawCommands(RobotCommandDto commands)
    {
        _logger.LogInformation("Processing message from server with new robot commands");

        await SendCommands(commands);
    }

    public async Task StartPicking(CreateOrderCommand createOrderCommand)
    {
        _logger.LogInformation("Processing message from server with new products to pick");

        if (_robotState.Event != null)
        {
            _logger.LogError("You cannot send commands if the robot is already processing some");
            throw new InvalidOperationException("You cannot send commands if the robot is already processing some");
        }

        List<Position> robotStops = PrepareRobotStops(createOrderCommand.OrderedProducts);
        TspAlgorithmResult result = _algorithmProvider.GetAlgorithm(createOrderCommand.TspAlgorithm).FindPath(robotStops);

        DirectionEnum startDirection = _robotState.Direction;
        List<RobotCommand> moves = GenerateCommands(result.Path, startDirection, createOrderCommand.OrderedProducts, result.Distances);

        RobotCommandDto commands = new RobotCommandDto() { Commands = moves };

        Order order = new Order()
        {
            OrderId = Guid.NewGuid(),
            OrderedProducts = createOrderCommand.OrderedProducts,
            PickedProducts = new List<OrderedProduct>(),
            TspAlgorithm = createOrderCommand.TspAlgorithm,
            Timestamp = createOrderCommand.Timestamp,
            Distance = result.TotalWeight,
            TspAlgorithmResults = result,
        };

        await _ordersRepository.AddAsync(order);
        _robotState.StartPicking(order.ToDto());
        
        await SendCommands(commands);
    }

    public async Task OrderAgain(OrderAgainCommand orderAgainCommand)
    {
        _logger.LogInformation("Processing message from server with new products to pick");

        if (_robotState.Event != null)
        {
            _logger.LogError("You cannot send commands if the robot is already processing some");
            throw new InvalidOperationException("You cannot send commands if the robot is already processing some");
        }

        Order prevOrder = await _ordersRepository.GetByIdAsync(orderAgainCommand.OrderId);
        //TODO: to remove
        _logger.LogError("Started copying ordered producta");
        List<OrderedProduct> orderedProducts = prevOrder.OrderedProducts
            .Select(p => new OrderedProduct
            {
                Id = p.Id,
                Name = p.Name,
                Quantity = 1,
                Position = new Position 
                { 
                    X = p.Position.X, 
                    Y = p.Position.Y 
                }
            })
            .ToList();

        _logger.LogError("Started prepering robot stops");
        List<Position> robotStops = PrepareRobotStops(orderedProducts);
        TspAlgorithmResult result;
        if (prevOrder.TspAlgorithmResults == null || prevOrder.TspAlgorithm != orderAgainCommand.TspAlgorithm)
        {
            _logger.LogInformation($"Have to calculate path again. TspAlgorithmResults is {prevOrder.TspAlgorithmResults} and {prevOrder.TspAlgorithm} != {orderAgainCommand.TspAlgorithm}");
            result = _algorithmProvider.GetAlgorithm(orderAgainCommand.TspAlgorithm).FindPath(robotStops);
        }
        else
        {
            result = prevOrder.TspAlgorithmResults with 
            {
                Id = Guid.NewGuid(),
                Path = prevOrder.TspAlgorithmResults.Path.Select(p => new TspPosition
                {
                    Id = Guid.NewGuid(),
                    X = p.X,
                    Y = p.Y,
                    OrderNumber = p.OrderNumber
                }).ToList()
            };
            _logger.LogInformation($"Using prev path: { string.Join(" -> ", result.Path.Select(p => $"({p.X},{p.Y})")) }");
        }

        DirectionEnum startDirection = _robotState.Direction;
        List<RobotCommand> moves = GenerateCommands(result.Path, startDirection, orderedProducts, result.Distances);

        RobotCommandDto commands = new RobotCommandDto() { Commands = moves };

        _logger.LogInformation($"Saving result: { string.Join(" -> ", result.Path.Select(p => $"({p.X},{p.Y})")) }");
        Order order = new Order()
        {
            OrderId = Guid.NewGuid(),
            OrderedProducts = orderedProducts,
            PickedProducts = new List<OrderedProduct>(),
            TspAlgorithm = orderAgainCommand.TspAlgorithm,
            Timestamp = orderAgainCommand.Timestamp,
            Distance = result.TotalWeight,
            TspAlgorithmResults = result,
        };

        await _ordersRepository.AddAsync(order);
        _robotState.StartPicking(order.ToDto());
        
        await SendCommands(commands);
    }

    public async Task SendStop()
    {
        _logger.LogInformation("Processing message from server to stop the robot");
        await _mqttProducer.PublishAsync(MqttTopics.RobotStop, string.Empty);
    }

    private async Task SendCommands(RobotCommandDto commands)
    {
        if (_robotState.Event != null)
        {
            _logger.LogError("You cannot send commands if the robot is already processing some");
            throw new InvalidOperationException("You cannot send commands if the robot is already processing some");
        }

        string message = Serializer.Serialize(commands);
        await _mqttProducer.PublishAsync(MqttTopics.RobotCommand, message);
    }

    private List<Position> PrepareRobotStops(List<OrderedProduct> orderedProducts)
    {
        Position startPosition = _robotState.CurrentPosition;
        Position finishPosition = startPosition;

        List<Position> positionsToSee = new List<Position>();

        List<Position> productPositions = orderedProducts.Select(product => product.Position).ToList();
        positionsToSee.Add(startPosition);
        positionsToSee.AddRange(productPositions);
        positionsToSee.Add(finishPosition);

        return positionsToSee;
    }

    private List<RobotCommand> GenerateCommands(List<TspPosition> positions, DirectionEnum startDirection, List<OrderedProduct> orderedProducts, List<double> distances)
    {
        _logger.LogInformation("Generating robot moves from positions");

        List<RobotCommand> moves = new List<RobotCommand>();
        List<OrderedProduct> productsToPick = new List<OrderedProduct>(orderedProducts);

        DirectionEnum oldDirection = startDirection;

        for (int i = 0; i < positions.Count - 1; i++)
        {
            int x_prev = positions[i].X;
            int y_prev = positions[i].Y;

            int x_next = positions[i+1].X;
            int y_next = positions[i+1].Y;

            OrderedProduct? orderedProductOnPosition = productsToPick.SingleOrDefault(orderedProduct => orderedProduct.Position.X == positions[i].X && orderedProduct.Position.Y == positions[i].Y);
            if (orderedProductOnPosition != null)
            {
                RobotCommand stopCommand = new RobotCommand() { Move = RobotMoveEnum.Stop, StopDurationMs = orderedProductOnPosition.Quantity * StopDurationMsForOne, OrderedProduct = orderedProductOnPosition };
                moves.Add(stopCommand);
                productsToPick.Remove(orderedProductOnPosition);
            }

            DirectionEnum newDirection = RobotOperation.FindNewDirection(x_prev, y_prev, x_next, y_next);
            RobotMoveEnum newMove = RobotOperation.GenerateMove(oldDirection, newDirection);

            RobotCommand command = new RobotCommand() { Move = newMove, Distance = distances[i]};
            moves.Add(command);

            oldDirection = newDirection;
        }

        _logger.LogInformation($"Generated moves: { string.Join(" -> ", moves) }");
        return moves;
    }
}