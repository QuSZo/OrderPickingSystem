using Api.Logging;
using Api.RobotOperations;
using Python.Runtime;

namespace Api.TravelingSalesmanAlgorithms.NetworkxAlgorithms; 

public class NetworkxAlgorithm : ITravelingSalesmanAlgorithm
{
    private readonly ILogger _logger;

    public NetworkxAlgorithm(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLoggerApi();
    }

    public TspAlgorithmResult FindPath(List<Position> positions)
    {
        List<Position> path = new List<Position>();
        double totalWeight;
        List<double> distances = new List<double>();

        using (Py.GIL())
        {
            var pythonPositions = new PyList();

            foreach (var position in positions)
            {
                pythonPositions.Append(new PyTuple(new PyObject[]
                {
                    position.X.ToPython(),
                    position.Y.ToPython()
                }));
            }

            dynamic script = Py.Import("TravelingSalesmanAlgorithms.NetworkxAlgorithms.NetworkxAlgorithmScript");
            dynamic result = script.find_path(pythonPositions);

            dynamic pyPath = result[0];
            totalWeight = result[1];
            dynamic pyDistances = result[2];

            foreach (PyObject node in pyPath)
            {
                int x = node[0].As<int>();
                int y = node[1].As<int>();

                path.Add(new Position() { X = x, Y = y });
            }

            foreach (PyObject cost in pyDistances)
            {
                distances.Add(cost.As<double>());
            }

            _logger.LogInformation($"Python script has been executed. Total distance: {totalWeight}, found path: { string.Join(" -> ", path.Select(p => $"({p.X},{p.Y})")) }");
        }

        return new TspAlgorithmResult() { Path = path, TotalWeight = totalWeight, Distances = distances};
    }
}