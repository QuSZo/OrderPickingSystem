using Api.RobotOperations;

namespace Api.Dtos;

public record OrderedProductDto
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required Position Position { get; init; } 
    public required int Quantity { get; init; } 
}