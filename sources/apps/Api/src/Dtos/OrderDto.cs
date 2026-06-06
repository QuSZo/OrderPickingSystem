using Api.Orders;
using Api.TravelingSalesmanAlgorithms;

namespace Api.Dtos; 

public record OrderDto
{
    public required Guid OrderId { get; init; }
    public required List<OrderedProduct> OrderedProducts { get; init; }
    public required List<OrderedProduct> PickedProducts { get; init; }
    public required TspAlgorithmsEnum TspAlgorithm { get; init; }
    public required double Timestamp { get; init; } 
    public required double Distance { get; init; }
    public long? StartPickingTime { get; set; }
    public long? FinishPickingTime { get; set; }
    public double? ProportionalAbsoluteMean { get; set; }
    public double? DerivativeAbsoluteMean { get; set; }
    public double? IntegralAbsoluteMean { get; set; }
    public double? PowerDifferenceAbsoluteMean { get; set; }
}