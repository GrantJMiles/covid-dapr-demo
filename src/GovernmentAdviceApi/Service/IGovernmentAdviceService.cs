namespace GovernmentAdviceApi; //{}

using CovidSimulator.Extensions.Data;
public interface IGovernmentAdviceService
{
    GovernmentAdvice GetLatestAdvice();
}