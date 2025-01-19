using UsaCensus.BackgroundTasks.Processors;

namespace UsaCensus.BackgroundTasks.Workers;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    private readonly IUsaCensusProcessor usaCensusProcessor;

    public Worker(ILogger<Worker> logger, IUsaCensusProcessor usaCensusProcessor)
    {
        _logger = logger;
        this.usaCensusProcessor = usaCensusProcessor;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }

            await this.usaCensusProcessor.ProcessCountiesDemographicsAsync();

            await Task.Delay(15000, stoppingToken);
        }
    }
}
