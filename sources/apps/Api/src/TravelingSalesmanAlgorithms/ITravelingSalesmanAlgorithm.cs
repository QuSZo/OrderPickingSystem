using Api.RobotOperations;

namespace Api.TravelingSalesmanAlgorithms;

public interface ITravelingSalesmanAlgorithm
{
    TspAlgorithmResult FindPath(List<Position> positions);
}