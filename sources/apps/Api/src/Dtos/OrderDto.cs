using Api.Orders;
using Api.TravelingSalesmanAlgorithms;

namespace Api.Dtos;

public record OrderDto
{
    public required List<OrderedProduct> OrderedProducts { get; init; }
    public required TspAlgorithmsEnum TspAlgorithm { get; init; }
    public required double Timestamp { get; init; } 
}