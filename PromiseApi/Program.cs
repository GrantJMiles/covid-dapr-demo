using Promises;
using Dapr.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


    app.UseSwagger();
    app.UseSwaggerUI();

//app.UseHttpsRedirection();
var _logger = app.Services.GetService<ILogger<Program>>() ?? throw new Exception("ERRRRRRSSSS");
var dClient = new DaprClientBuilder().Build();

app.MapPost("/create", (MyPromise promise) => {
    var guid = Guid.NewGuid().ToString();
    dClient.SaveStateAsync("test-state", guid, promise);
    return guid;
})
.WithName("CreatePromise");
app.MapGet("/find", (string id) => dClient.GetStateAsync<MyPromise>("test-state", id)).WithName("FindPromise");
app.MapPut("/start", async (string id) => {
    _logger.LogInformation("Starting publish");
    var prom = await dClient.GetStateAsync<MyPromise>("test-state", id);
    _logger.LogInformation("Got State from store {promise}", prom);
    await dClient.PublishEventAsync<MyPromise>("test-pubsub", "GrantsPromise", prom);
    return id;
})
.WithMetadata()
.WithName("StartPromise");

app.Run();



record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}