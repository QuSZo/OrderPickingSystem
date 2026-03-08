namespace Api.Products;

public record Position
{
    public required int X { get; init; }
    public required int Y { get; init; }
}