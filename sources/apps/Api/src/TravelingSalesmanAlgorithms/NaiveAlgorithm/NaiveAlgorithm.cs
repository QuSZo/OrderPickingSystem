using Api.Logging;
using Api.Products;
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
        List<Position> elements = new List<Position>();

        using (Py.GIL())
        {
            dynamic script = Py.Import("TravelingSalesmanAlgorithms.NaiveAlgorithm.NaiveAlgorithmScript");
            dynamic result = script.find_path();
            _logger.LogInformation("Python script has been executed");
        }

        return elements;
    }
}