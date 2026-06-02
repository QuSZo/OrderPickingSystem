namespace Api.RobotOperations;

public record RobotPIDSummary
{
    public required List<double> ProportionalHistory { get; init; }
    public required List<double> DerivativeHistory { get; init; }
    public required List<double> IntegralHistory { get; init; }
    public required List<double> PowerDifferenceHistory { get; init; }
}