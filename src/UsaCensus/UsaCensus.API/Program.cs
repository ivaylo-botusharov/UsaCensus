using UsaCensus.API.Endpoints;
using UsaCensus.API.Models;
using UsaCensus.API.Repositories;
using UsaCensus.API.Initializers;
using UsaCensus.API.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.Configure<UsaCensusDatabaseSettings>(
    builder.Configuration.GetSection("UsaCensusDatabase"));

builder.Services.AddOpenApi();

builder.Services.AddHttpClient();

builder.Services.AddSingleton<IDemographicsRepository, DemographicsRepository>();

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
