using CovidSimulator.Extensions.Data;

namespace GovernmentAdviceApi; //{}

public class GovernmentAdviceService : IGovernmentAdviceService
{
    private readonly ILogger<GovernmentAdviceService> _logger;

    public GovernmentAdviceService(ILogger<GovernmentAdviceService> logger)
    => (_logger) = (logger);

    public GovernmentAdvice GetLatestAdvice()
    {
        _logger.LogDebug("Getting Latest Government Advice");
        var govInfo = new GovernmentAdvice(14, 2, LockdownLevel.AlertLevelTwo);
        LogAdviceReceived(govInfo);
        return govInfo;
    }

    private void LogAdviceReceived(GovernmentAdvice govInfo) => _logger.LogInformation("Recieved latest advice with : {isolationDays} days, {vaccineTotal} vaccinations, {LockdownLevel}", govInfo.CurrentDaysInIsolation, govInfo.TotalNumberOfAvailableVaccinations, govInfo.LockdownLevel);
}