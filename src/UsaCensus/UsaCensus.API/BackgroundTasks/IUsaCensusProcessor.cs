namespace UsaCensus.API.BackgroundTasks;

public interface IUsaCensusProcessor
{
    Task ProcessCountiesDemographics();
}
