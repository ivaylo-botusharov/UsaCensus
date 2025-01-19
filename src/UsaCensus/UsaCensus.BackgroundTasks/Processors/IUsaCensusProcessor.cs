namespace UsaCensus.BackgroundTasks.Processors;

public interface IUsaCensusProcessor
{
    Task ProcessCountiesDemographicsAsync();
}
