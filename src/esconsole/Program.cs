using EventStore.Client;
using System.Text;
using System.Text.Json;


var settings = EventStoreClientSettings
        .Create("esdb://host.docker.internal:2113?tls=false");
var client = new EventStoreClient(settings);


var eventData = new EventData(
    Uuid.NewUuid(),
    "TestEvent",
    JsonSerializer.SerializeToUtf8Bytes(new {Hello = "World!"})
);


System.Console.WriteLine("Starting Write");

await client.AppendToStreamAsync(
    "some-stream",
    StreamState.Any,
    new[] { eventData }
);

System.Console.WriteLine("Written");