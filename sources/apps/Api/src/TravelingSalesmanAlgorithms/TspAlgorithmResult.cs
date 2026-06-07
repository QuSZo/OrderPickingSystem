using Api.RobotOperations;

namespace Api.TravelingSalesmanAlgorithms;

public record TspAlgorithmResult
{
    public required Guid Id { get; init; }
    public required List<TspPosition> Path { get; init; }
    public required double TotalWeight { get; init; }
    public required List<double> Distances { get; init; }
}