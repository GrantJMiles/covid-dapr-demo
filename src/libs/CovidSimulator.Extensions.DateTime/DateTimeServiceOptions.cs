namespace CovidSimulator.Extensions.DateTime;

public class DateTimeServiceOptions
{
    public DateOnly StartingDateForService { get; init; }
    public int HitCountToIncrementDate { get; init; }
}