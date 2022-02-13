using CovidSimulator.Extensions.Data;
using CovidSimulator.Extensions.DateTimeService;
using CovidTestingLabApi;

public class CovidResultService : ICovidResultService
{
    private readonly ILogger<CovidResultService> _logger;
    private readonly IDateTimeService _datetimeService;
    private readonly Random _random;

    public CovidResultService(ILogger<CovidResultService> logger, IDateTimeService dateTimeService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _datetimeService = dateTimeService ?? throw new ArgumentNullException(nameof(dateTimeService));
        _random = new Random();
    }
    public CovidSampleResult GetSampleResult(CovidPatient patient, GovernmentAdvice latestAdvice)
    {
        var negativeChance = GetDoubleChanceOfNegativeResult(patient, latestAdvice);
        var isNegative = DoesTheTestShowANegativeResult(negativeChance);
        _logger.LogInformation("Covid result returned, not covid flag: {isResultNegative}", isNegative);
        return new CovidSampleResult(patient.NhsNumber, _datetimeService.GetCurrentDate(), !isNegative);
    }

    private bool DoesTheTestShowANegativeResult(double constuctedChanceValue) 
    => constuctedChanceValue < _random.NextDouble();
    private double GetDoubleChanceOfNegativeResult(CovidPatient patient, GovernmentAdvice latestAdvice)
    {
        _logger.LogInformation("Calcualting covid chance based on Current Number of Days Isolation ({noOfDaysIsolating}) and Current Lockdown Level ({lockdownLevel})", latestAdvice.CurrentDaysInIsolation, latestAdvice.LockdownLevel);
        var isFullyVaccinated = IsPatientFullyVaccinated(patient, latestAdvice);
        var negativeChance = GetCalculationFromIsolationDaysAndLevel(latestAdvice);
        if (!isFullyVaccinated) 
        { 
            _logger.LogInformation("Reducing chances of negative results due to vaccination status");
            negativeChance = negativeChance / 2; 
        }
        return negativeChance;
    }
    private double GetCalculationFromIsolationDaysAndLevel(GovernmentAdvice advice)
    => (advice.CurrentDaysInIsolation * (int) advice.LockdownLevel) / 100;

    private bool IsPatientFullyVaccinated(CovidPatient patient, GovernmentAdvice govAdvice) 
    => patient.numberOfVaccinations == govAdvice.TotalNumberOfAvailableVaccinations;
}