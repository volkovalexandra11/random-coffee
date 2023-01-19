namespace RandomCoffeeServer.Domain.Services.Coffee.Rounds;

// Один раз Проводит раунды в созревших группах
public class ScheduledRoundsMakerService
{
    public ScheduledRoundsMakerService(ILogger<ScheduledRoundsMakerService> log)
    {
        this.log = log;
    }

    public async Task MakeRound(CancellationToken cancellationToken)
    {
        log.LogInformation("Making round");
    }

    private readonly ILogger<ScheduledRoundsMakerService> log;
}