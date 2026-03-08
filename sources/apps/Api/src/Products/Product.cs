namespace Api.Products;

public record Product
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required Position Position { get; init; } 
}