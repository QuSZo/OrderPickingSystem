using Api.Logging;
using Api.RobotOperations;
using Python.Runtime;

namespace Api.TravelingSalesmanAlgorithms.NaiveAlgorithms; 

public class NaiveAlgorithm : ITravelingSalesmanAlgorithm
{
    private readonly ILogger _logger;

    public NaiveAlgorithm(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLoggerApi();
    }

    public TspAlgorithmResult FindPath(List<Position> positions)
    {
        List<TspPosition> path = new List<TspPosition>();
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

            dynamic script = Py.Import("TravelingSalesmanAlgorithms.NaiveAlgorithms.NaiveAlgorithmScript");
            dynamic result = script.find_path(pythonPositions);

            dynamic pyPath = result[0];
            totalWeight = result[1];
            dynamic pyDistances = result[2];

            int orderNumber = 0;

            foreach (PyObject node in pyPath)
            {
                int x = node[0].As<int>();
                int y = node[1].As<int>();

                path.Add(new TspPosition() { Id = Guid.NewGuid(), X = x, Y = y, OrderNumber = orderNumber });

                orderNumber++;
            }

            foreach (PyObject cost in pyDistances)
            {
                distances.Add(cost.As<double>());
            }

            _logger.LogInformation($"Python script has been executed. Total distance: {totalWeight}, found path: { string.Join(" -> ", path.Select(p => $"({p.X},{p.Y})")) }");
        }

        return new TspAlgorithmResult() { Id = Guid.NewGuid(), Path = path, TotalWeight = totalWeight, Distances = distances};
    }
}