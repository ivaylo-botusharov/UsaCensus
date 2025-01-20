using UsaCensus.BackgroundTasks.Processors;

namespace UsaCensus.BackgroundTasks.Workers;

public class UsaCensusCountiesWorker : BackgroundService
{
    private readonly ILogger<UsaCensusCountiesWorker> _logger;

    private readonly IUsaCensusProcessor usaCensusProcessor;

    public UsaCensusCountiesWorker(ILogger<UsaCensusCountiesWorker> logger, IUsaCensusProcessor usaCensusProcessor)
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
                _logger.LogInformation("UsaCensusCountiesWorker running at: {time}", DateTimeOffset.Now);
            }

            await this.usaCensusProcessor.ProcessCountiesDemographicsAsync();

            await Task.Delay(15000, stoppingToken);
        }
    }
}
