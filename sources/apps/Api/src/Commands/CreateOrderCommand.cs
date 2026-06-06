using Api.Orders;
using Api.TravelingSalesmanAlgorithms;

namespace Api.Commands;

public record CreateOrderCommand
{
    public required List<OrderedProduct> OrderedProducts { get; init; }
    public required TspAlgorithmsEnum TspAlgorithm { get; init; }
    public required double Timestamp { get; init; } 
}