using UsaCensus.API.Endpoints;
using UsaCensus.API.Models;
using UsaCensus.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<UsaCensusDatabaseSettings>(
    builder.Configuration.GetSection("UsaCensusDatabase"));

builder.Services.AddOpenApi();

builder.Services.AddSingleton<DemographicsService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapDemographicsEndpoints();

app.Run();
