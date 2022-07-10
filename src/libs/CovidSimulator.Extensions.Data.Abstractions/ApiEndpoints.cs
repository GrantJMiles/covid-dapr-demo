namespace CovidSimulator.Extensions.Data;//{}

public static class ApiEndpoints
{
    public const string SubmitCovidSampleCreate = "api/covid-lab/sample/create";
    public const string SubmitCovidSampleGet = "api/covid-lab/sample/get";
    public const string SubmitCovidSampleStart = "api/covid-lab/sample/start";
    public const string CovidSampleConsumer = "sub/covid-lab/process-sample";
    public const string CovidPositiveConsumer = "sub/test-trace/positive";
}