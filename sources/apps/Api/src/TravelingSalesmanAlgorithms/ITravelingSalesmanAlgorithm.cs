using Api.RobotOperations;

namespace Api.TravelingSalesmanAlgorithms;

public interface ITravelingSalesmanAlgorithm
{
    List<Position> FindPath(List<Position> positions);
}