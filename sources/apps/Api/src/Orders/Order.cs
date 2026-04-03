using Api.TravelingSalesmanAlgorithms;

namespace Api.Orders;

public record Order
{
    public required Guid OrderId { get; init; }
    public required List<OrderedProduct> OrderedProducts { get; init; }
    public required TspAlgorithmsEnum TspAlgorithm { get; init; }
    public required double Timestamp { get; init; } 
    public required double Distance { get; init; }
    public required double StartPickingTime { get; init; }
    public double? FinishPickingTime { get; set; }
}