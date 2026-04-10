using Api.RobotOperations;

namespace Api.TravelingSalesmanAlgorithms;

public record TspAlgorithmResult
{
    public required List<Position> Path { get; init; }
    public required double TotalWeight { get; init; }
    public required List<double> Distances { get; init; }
}