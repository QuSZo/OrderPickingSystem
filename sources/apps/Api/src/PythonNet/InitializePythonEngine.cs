using Python.Runtime;

namespace Api.PythonNet;

public class InitializePythonEngine : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        PythonEngine.Initialize();
        PythonEngine.BeginAllowThreads();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}