using Microsoft.Extensions.Options;

using UsaCensus.BackgroundTasks.Models;
using UsaCensus.BackgroundTasks.Processors;
using UsaCensus.BackgroundTasks.ServiceDefaults;
using UsaCensus.BackgroundTasks.Workers;
using UsaCensus.Infrastructure.Database.Initializers;
using UsaCensus.Infrastructure.Database.Models;
using UsaCensus.Infrastructure.Database.Repositories;
using UsaCensus.Infrastructure.Http;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Configuration
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile("Configuration/appsettings-shared.json", optional: false, reloadOnChange: false);

builder.Services.Configure<UsaCensusDatabaseSettings>(
    builder.Configuration.GetSection(UsaCensusDatabaseSettings.SectionName));

builder.Services.Configure<ArcGisUrlSettings>(
    builder.Configuration.GetSection(ArcGisUrlSettings.SectionName));

builder.Services.AddHttpClient();

builder.AddServiceDefaults();

builder.Services.AddSingleton<IHttpClientWrapper, HttpClientWrapper>();

builder.Services.AddSingleton<IDemographicsRepository, DemographicsRepository>();

builder.Services.AddHostedService<UsaCensusCountiesWorker>();
builder.Services.AddSingleton<IUsaCensusProcessor, UsaCensusProcessor>();

IHost host = builder.Build();

UsaCensusDatabaseSettings? dbSettings = builder.Configuration
    .GetSection(UsaCensusDatabaseSettings.SectionName)
    .Get<UsaCensusDatabaseSettings>();

DatabaseInitializer databaseInitializer = new(dbSettings);
databaseInitializer.Initialize();

host.Run();
