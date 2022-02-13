using CovidSimulator.Extensions.Data;
using CovidSimulator.Extensions.DateTimeService;
using CovidTestingLabApi;
using Dapr.Client;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using EventStore.Client;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<ICovidResultService, CovidResultService>();
builder.Services.AddTransient<IDateTimeService, DateTimeService>();
builder.Services.AddControllers().AddDapr();
builder.Services.Configure<JsonOptions>(options =>
{
    options.JsonSerializerOptions.IncludeFields = true;
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseCloudEvents();
app.MapControllers();
app.MapSubscribeHandler();
// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();
var dClient = new DaprClientBuilder().Build();
var logger = app.Services.GetService<ILogger<Program>>() ?? throw new InvalidOperationException("cannot resolve logger");
var covidService = app.Services.GetService<ICovidResultService>() ?? throw new InvalidOperationException("cannot resolve covid service");

var stateStore = app.Configuration.GetValue<string>("Dapr:StateStore");
var pubsub = app.Configuration.GetValue<string>("Dapr:PubSub");
logger.LogInformation($"Getting StateStore name: {stateStore}");


// REPLACE WITH THE DATETIME SERVICE ONCE UP UP RUNNING
app.MapPost(ApiEndpoints.SubmitCovidSampleCreate, async (CovidPatient patient) =>
{
    var aRequest = new ActionRequest<CovidPatient>(patient.NhsNumber, DateTime.Now, patient);
    await dClient.SaveStateAsync(stateStore, aRequest.id, aRequest);
    return Results.Created(ApiEndpoints.SubmitCovidSampleGet, aRequest);
}).Produces(StatusCodes.Status201Created);

app.MapGet(ApiEndpoints.SubmitCovidSampleGet, async (string nhsNumber) => await GetRequest(dClient, stateStore, nhsNumber));

app.MapPut(ApiEndpoints.SubmitCovidSampleStart, async (string nhsNumber) =>
{
    var aRequest = await dClient.GetStateAsync<ActionRequest<CovidPatient>>(stateStore, nhsNumber);
    logger.LogInformation("Get ActionRequest from State Store for {nhsNumber}", aRequest.data.NhsNumber);
    var command = new TestCovidPatientCommand(aRequest.data.Name, aRequest.data.NhsNumber, aRequest.data.numberOfVaccinations);
    aRequest = aRequest with { StartedAt = DateTime.Now, started = true };
    logger.LogInformation("Publishing TestCovidPatientCommand for {nhsNumber}", aRequest.data.NhsNumber);
    await dClient.PublishEventAsync<TestCovidPatientCommand>(pubsub, "testForCovid", command);
});

app.MapPost(ApiEndpoints.CovidSampleConsumer, async (TestCovidPatientCommand patientToProcess) =>
{
    if (patientToProcess == default) throw new ArgumentException(nameof(patientToProcess));
    logger.LogInformation("patientToProcess: {patient}", patientToProcess);
    logger.LogInformation("Starting to process lab results for {nhsNumber}", patientToProcess.NhsNumber);
    var request = dClient.CreateInvokeMethodRequest(HttpMethod.Get, "govadviceapi", "api/gov/advice");
    var latestGovAdvice = await dClient.InvokeMethodAsync<GovernmentAdvice>(request);
    var covidResults = covidService.GetSampleResult(patient: patientToProcess, latestAdvice: latestGovAdvice);
    var evt = new EventData(
        eventId: Uuid.NewUuid(),
        type: "covid-result",
        data: Encoding.UTF8.GetBytes(JsonSerializer.Serialize(covidResults))
    );

    logger.LogInformation("Starting Write to ESDB");
    var settings = EventStoreClientSettings
        .Create("esdb://eventstoredb:2113?tls=false&keepAliveTimeout=10000&keepAliveInterval=10000");
    var client = new EventStoreClient(settings);
    var result = await client.AppendToStreamAsync("covid-result-v1", StreamState.Any, new [] {evt});
    logger.LogInformation("Event Store Result pos {eventCommitPosition}", result.LogPosition.CommitPosition);
    if (covidResults.HasCovid)
    {
        await dClient.PublishEventAsync<CovidSampleResult>(pubsub, "positive-result", covidResults);
    }
});

app.Run();

static async Task<ActionRequest<CovidPatient>> GetRequest(DaprClient client, string stateStore, string key) => await client.GetStateAsync<ActionRequest<CovidPatient>>(stateStore, key);