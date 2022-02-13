namespace CovidSimulator.Extensions.Data;// {}

public record CovidSampleResult(string NhsNumber, DateTime DateCompleted, bool HasCovid)
{
    public static CovidSampleResult Pass(string nhsNumber, DateTime completedDate) => new CovidSampleResult(nhsNumber, completedDate, true);
    public static CovidSampleResult Fail(string nhsNumber, DateTime completedDate) => new CovidSampleResult(nhsNumber, completedDate, false);
}