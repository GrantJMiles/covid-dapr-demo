namespace CovidSimulator.Extensions.DateTime; //{}

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class DateTimeServiceExtensions
{
    public static IServiceCollection AddDateTimeService(this IServiceCollection services, IConfiguration configurationSection)
    => services.Configure<DateTimeServiceOptions>(configurationSection);
}