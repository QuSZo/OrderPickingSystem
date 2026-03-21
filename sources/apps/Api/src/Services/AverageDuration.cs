namespace Api.Services;

public record AverageDuration
{
    public required string Algorithm { get; init; }
    public required double AverageDurationMs { get; init; }
}