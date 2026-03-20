using Api.TravelingSalesmanAlgorithms;

namespace Api.Orders;

public record Order
{
    public required List<OrderedProduct> OrderedProducts { get; init; }
    public required TspAlgorithmsEnum TspAlgorithm { get; init; }
    public required double Timestamp { get; init; } 
}