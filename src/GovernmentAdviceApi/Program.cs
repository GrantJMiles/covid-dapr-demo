using CovidSimulator.Extensions.Data;
using Dapr.Client;
using GovernmentAdviceApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IGovernmentAdviceService, GovernmentAdviceService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

//app.UseHttpsRedirection();

var dClient = new DaprClientBuilder().Build();
var govAdviceService = app.Services.GetService<IGovernmentAdviceService>() ?? throw new ArgumentException("Unable to resolve IGovernmentAdviceService");

app.MapGet("api/gov/advice", () => govAdviceService.GetLatestAdvice())
    .Produces<GovernmentAdvice>(StatusCodes.Status200OK)
    .WithName("GetLatestGovernmentAdvice")
    .WithTags("GovernmentAdvice");

app.Run();