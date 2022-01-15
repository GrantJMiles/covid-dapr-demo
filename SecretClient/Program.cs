// See https://aka.ms/new-console-template for more information
using Dapr.Client;

Console.WriteLine("Hello, World!");
Console.WriteLine("Hello, World!");

var client = new DaprClientBuilder().Build();
//var secrets = await client.GetSecretAsync("my-secret-store","my-secret");

//System.Console.WriteLine($"Got Secret Keys from JSON file: {string.Join(",", secrets.Keys)}");
//System.Console.WriteLine($"My Secret: {(secrets["my-secret"])}");

Console.WriteLine("Starting to store state");
var guid = Guid.NewGuid();
System.Console.WriteLine($"Saving state for id: {guid}");
await client.SaveStateAsync("test-state", guid.ToString(), new Promises.MyPromise(guid, "Grant Miles", DateTime.Now));

var x = await client.GetStateAsync<Promises.MyPromise>("test-state", guid.ToString());

System.Console.WriteLine($"Hello {x.Name}");

await client.PublishEventAsync("test-pubsub", guid.ToString(), x);
System.Console.WriteLine("published");