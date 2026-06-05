using Api.TravelingSalesmanAlgorithms;

namespace Api.Orders;

public record Order
{
    public required Guid OrderId { get; init; }
    public required List<OrderedProduct> OrderedProducts { get; init; }
    public required List<OrderedProduct> PickedProducts { get; init; }
    public required TspAlgorithmsEnum TspAlgorithm { get; init; }
    public required double Timestamp { get; init; } 
    public required double Distance { get; init; }
    public long? StartPickingTime { get; set; }
    public long? FinishPickingTime { get; set; }
    public List<double>? ProportionalHistory { get; set; }
    public List<double>? DerivativeHistory { get; set; }
    public List<double>? IntegralHistory { get; set; }
    public List<double>? PowerDifferenceHistory { get; set; }
    public double? ProportionalAbsoluteMean { get; set; }
    public double? DerivativeAbsoluteMean { get; set; }
    public double? IntegralAbsoluteMean { get; set; }
    public double? PowerDifferenceAbsoluteMean { get; set; }
}