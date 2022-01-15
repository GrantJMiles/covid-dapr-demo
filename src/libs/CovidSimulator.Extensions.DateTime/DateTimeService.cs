namespace CovidSimulator.Extensions.DateTime;//{}

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

public class DateTimeService : IDateTimeService
{
    private readonly ILogger<DateTimeService> _logger;
    private readonly IOptions<DateTimeServiceOptions> _options;
    private DateOnly _dateToReturn;
    private int _hitCountForDayIncrement;
    private int _numberOfHits;
    public DateTimeService(ILogger<DateTimeService> logger, IOptions<DateTimeServiceOptions> options)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _numberOfHits = 0;
    }

    public DateOnly GetCurrentDate()
    {
       IncrementDateIfNeeded();
       return _dateToReturn; 
    }
    private void IncrementDateIfNeeded()
    {
        if (_numberOfHits % _hitCountForDayIncrement == 0) { _dateToReturn.AddDays(1); }
    }
    private void SetDefaults()
    {
        _dateToReturn = _options?.Value?.StartingDateForService ?? new DateOnly(2019,02,17);
        _hitCountForDayIncrement = _options?.Value?.HitCountToIncrementDate ?? 100;
    }
}
