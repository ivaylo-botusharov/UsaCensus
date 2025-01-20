using UsaCensus.API.Endpoints;
using UsaCensus.Infrastructure.Initializers;
using UsaCensus.Infrastructure.Http;
using UsaCensus.Infrastructure.Models;
using UsaCensus.Infrastructure.Repositories;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile("Configuration/appsettings-shared.json", optional: false, reloadOnChange: false);

builder.Services.Configure<UsaCensusDatabaseSettings>(
    builder.Configuration.GetSection(UsaCensusDatabaseSettings.SectionName));

builder.Services.AddOpenApi();

builder.Services.AddSingleton<IDemographicsRepository, DemographicsRepository>();

builder.Services.AddLogging(builder => builder.AddConsole());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapDemographicsEndpoints();

app.Run();
