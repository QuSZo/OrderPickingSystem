using Api.Logging;
using Api.RobotOperations;
using Api.TravelingSalesmanAlgorithms;
using Python.Runtime;

public class NaiveAlgorithm : ITravelingSalesmanAlgorithm
{
    private readonly ILogger _logger;

    public NaiveAlgorithm(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLoggerApi();
    }

    public List<Position> FindPath(List<Position> positions)
    {
        List<Position> path = new List<Position>();

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

            dynamic script = Py.Import("TravelingSalesmanAlgorithms.NaiveAlgorithm.NaiveAlgorithmScript");
            dynamic result = script.find_path(pythonPositions);

            foreach (PyObject node in result)
            {
                int x = node[0].As<int>();
                int y = node[1].As<int>();

                path.Add(new Position() { X = x, Y = y });
            }

            _logger.LogInformation($"Python script has been executed. Found path: { string.Join(" -> ", path.Select(p => $"({p.X},{p.Y})")) }");
        }

        return path;
    }
}