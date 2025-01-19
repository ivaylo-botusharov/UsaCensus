using Microsoft.Extensions.Options;

using UsaCensus.BackgroundTasks.Models;
using UsaCensus.BackgroundTasks.Processors;
using UsaCensus.BackgroundTasks.ServiceDefaults;
using UsaCensus.BackgroundTasks.Workers;
using UsaCensus.Infrastructure.Http;
using UsaCensus.Infrastructure.Initializers;
using UsaCensus.Infrastructure.Models;
using UsaCensus.Infrastructure.Repositories;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile("Configuration/appsettings-shared.json", optional: false, reloadOnChange: false);

builder.Services.Configure<UsaCensusDatabaseSettings>(
    builder.Configuration.GetSection("UsaCensusDatabase"));

builder.Services.Configure<ArcGisUrlSettings>(
    builder.Configuration.GetSection("ArcGisUrlSettings"));

builder.Services.AddHttpClient();

builder.AddServiceDefaults();

builder.Services.AddSingleton<IHttpClientWrapper, HttpClientWrapper>();

builder.Services.AddSingleton<IDemographicsRepository, DemographicsRepository>();

builder.Services.AddHostedService<Worker>();
builder.Services.AddSingleton<IUsaCensusProcessor, UsaCensusProcessor>();

var host = builder.Build();

var dbSettings = builder.Configuration.GetSection("UsaCensusDatabase").Get<UsaCensusDatabaseSettings>();
DatabaseInitializer databaseInitializer = new(dbSettings);
databaseInitializer.Initialize();

host.Run();
