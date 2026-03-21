namespace Api.Services;

public record AverageDurationByOrderSize
{
    public required string Algorithm { get; init; }
    public required int ProductCount { get; init; }
    public required double AverageDurationMs { get; init; }
}