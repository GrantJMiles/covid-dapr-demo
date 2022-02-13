using CovidSimulator.Extensions.Data;

namespace CovidTestingLabApi; //{}

public interface ICovidResultService
{
    CovidSampleResult GetSampleResult(CovidPatient patient, GovernmentAdvice latestAdvice);
}