using UsaCensus.API.Endpoints;
using UsaCensus.Infrastructure.Database.Initializers;
using UsaCensus.Infrastructure.Database.Models;
using UsaCensus.Infrastructure.Database.Repositories;
using UsaCensus.Infrastructure.Http;
using Microsoft.Extensions.Options;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile("Configuration/appsettings-shared.json", optional: false, reloadOnChange: false);

builder.Services.Configure<UsaCensusDatabaseSettings>(
    builder.Configuration.GetSection(UsaCensusDatabaseSettings.SectionName));

builder.Services.AddOpenApi();

builder.Services.AddSingleton<IDemographicsRepository, DemographicsRepository>();

builder.Services.AddLogging(builder => builder.AddConsole());

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapDemographicsEndpoints();

app.Run();
