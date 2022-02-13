namespace CovidSimulator.Extensions.DateTimeService;//{}

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

public class DateTimeService : IDateTimeService
{
    private readonly ILogger<DateTimeService> _logger;
    private readonly IOptions<DateTimeServiceOptions> _options;
    private DateTime _dateToReturn;
    private int _hitCountForDayIncrement;
    private int _numberOfHits;
    public DateTimeService(ILogger<DateTimeService> logger, IOptions<DateTimeServiceOptions> options)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _numberOfHits = 0;
        SetDefaults();
    }

    public DateTime GetCurrentDate()
    {
       IncrementDateIfNeeded();
       return _dateToReturn; 
    }
    private void IncrementDateIfNeeded()
    {
        _numberOfHits++;
        if (_numberOfHits % _hitCountForDayIncrement == 0) { _dateToReturn.AddDays(1); }
    }
    private void SetDefaults()
    {
        _dateToReturn = _options?.Value?.StartingDateForService ?? new DateTime(2019,02,17);
        _hitCountForDayIncrement = 100;
    }
}
