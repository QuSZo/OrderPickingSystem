using Api.TravelingSalesmanAlgorithms;

namespace Api.Commands;

public record OrderAgainCommand
{
    public required Guid OrderId { get; init; }
    public required TspAlgorithmsEnum TspAlgorithm { get; init; }
    public required double Timestamp { get; init; } 
}