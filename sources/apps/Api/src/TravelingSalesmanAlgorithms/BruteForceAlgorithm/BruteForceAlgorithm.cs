using Api.Logging;
using Api.RobotOperations;
using Api.TravelingSalesmanAlgorithms;
using Python.Runtime;

public class BruteForceAlgorithm : ITravelingSalesmanAlgorithm
{
    private readonly ILogger _logger;

    public BruteForceAlgorithm(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLoggerApi();
    }

    public TspAlgorithmResult FindPath(List<Position> positions)
    {
        List<Position> path = new List<Position>();
        double totalWeight;

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

            dynamic script = Py.Import("TravelingSalesmanAlgorithms.BruteForceAlgorithm.BruteForceAlgorithmScript");
            dynamic result = script.find_path(pythonPositions);

            dynamic pyPath = result[0];
            totalWeight = result[1];

            foreach (PyObject node in pyPath)
            {
                int x = node[0].As<int>();
                int y = node[1].As<int>();

                path.Add(new Position() { X = x, Y = y });
            }

            _logger.LogInformation($"Python script has been executed. Total distance: {totalWeight}, found path: { string.Join(" -> ", path.Select(p => $"({p.X},{p.Y})")) }");
        }

        return new TspAlgorithmResult() { Path = path, TotalWeight = totalWeight};
    }
}