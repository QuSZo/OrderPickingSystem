using Api.RobotOperations;

namespace Api.Orders;

public record OrderedProduct
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required Position Position { get; init; } 
    public required int Quantity { get; init; } 
}