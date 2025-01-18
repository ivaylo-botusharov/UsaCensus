using UsaCensus.API.Endpoints;
using UsaCensus.API.Models;
using UsaCensus.API.Services;
using UsaCensus.API.Initializers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<UsaCensusDatabaseSettings>(
    builder.Configuration.GetSection("UsaCensusDatabase"));

builder.Services.AddOpenApi();

builder.Services.AddSingleton<DemographicsService>();

var app = builder.Build();

var dbSettings = builder.Configuration.GetSection("UsaCensusDatabase").Get<UsaCensusDatabaseSettings>();
DatabaseInitializer databaseInitializer = new(dbSettings);
databaseInitializer.Initialize();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapDemographicsEndpoints();

app.Run();
