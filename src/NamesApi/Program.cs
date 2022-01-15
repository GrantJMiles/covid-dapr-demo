using System.Collections.Immutable;
using extensions;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

var random = new Random();
app.MapGet("/getname", () => GetCovidPatient());
app.MapGet("/getnames", (int count) => {
    var list = ImmutableList<CovidPatient>.Empty;
    for(int i = 0; i < count; count++)
        list = list.Add(GetCovidPatient());
    return list;
});


app.Run();

CovidPatient GetCovidPatient() => new CovidPatient(Faker.Name.FullName(Faker.NameFormats.Standard), Faker.Identification.UkNhsNumber(), random.NextDouble() < 0.5 );