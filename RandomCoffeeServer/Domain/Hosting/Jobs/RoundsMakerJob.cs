namespace RandomCoffeeServer.Domain.Hosting.Jobs;

public class RoundsMakerJob : IHostedService
{
    public RoundsMakerJob(ILogger<RoundsMakerJob> log)
    {
        this.log = log;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        log.LogInformation($"Starting {nameof(RoundsMakerJob)}");
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
    }

    private readonly ILogger<RoundsMakerJob> log;
}