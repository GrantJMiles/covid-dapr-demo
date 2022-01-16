using CovidSimulator.Extensions.Data;
using CovidTestingLabApi;
using Dapr.Client;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();
var dClient = new DaprClientBuilder().Build();
var logger = app.Services.GetService<ILogger<Program>>() ?? throw new InvalidOperationException("cannot resolve logger");

var stateStore = app.Configuration.GetValue<string>("Dapr:StateStore");
var pubsub = app.Configuration.GetValue<string>("Dapr:PubSub");
logger.LogInformation($"Getting StateStore name: {stateStore}");

// REPLACE WITH THE DATETIME SERVICE ONCE UP UP RUNNING
app.MapPost(ApiEndpoints.SubmitCovidSampleCreate, async (CovidPatient patient) => {
    var aRequest = new ActionRequest<CovidPatient>(patient.NhsNumber, DateTime.Now, patient);
    await dClient.SaveStateAsync(stateStore, aRequest.id, aRequest);
    return Results.Created(ApiEndpoints.SubmitCovidSampleGet, aRequest);
}).Produces(StatusCodes.Status201Created);

app.MapGet(ApiEndpoints.SubmitCovidSampleGet, async (string nhsNumber) => await dClient.GetStateAsync<ActionRequest<CovidPatient>>(stateStore, nhsNumber));

app.MapPut(ApiEndpoints.SubmitCovidSampleStart, async (string nhsNumber) => {
    var aRequest = await dClient.GetStateAsync<ActionRequest<CovidPatient>>(stateStore, nhsNumber);
    aRequest = aRequest with { StartedAt = DateTime.Now, started = true};
    var command = aRequest?.data as TestCovidPatientCommand ?? throw new InvalidOperationException($"Unable to find data for {aRequest?.id}");
    await dClient.PublishEventAsync<TestCovidPatientCommand>(pubsub, "testForCovid", command);
});

app.Run();

record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}