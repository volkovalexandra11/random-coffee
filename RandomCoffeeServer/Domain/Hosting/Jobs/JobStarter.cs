namespace RandomCoffeeServer.Domain.Hosting.Jobs;

// need this class because container service configuration is overriden
// by .AddHostedService service configuration
public class JobStarter<TJob> : IHostedService
    where TJob : IHostedService
{
    public JobStarter(TJob job)
    {
        this.job = job;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await job.StartAsync(cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await job.StopAsync(cancellationToken);
    }

    private readonly IHostedService job;
}