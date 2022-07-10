using CovidSimulator.Extensions.Data;
using CovidSimulator.Extensions.DateTimeService;
using Dapr.Client;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using EventStore.Client;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddDapr();
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


app.MapPost(ApiEndpoints.CovidPositiveConsumer, (CovidSampleResult result) => {

});
app.Run();
