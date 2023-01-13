namespace RandomCoffeeServer.Domain.Services.Coffee;

public class SingleRoundMakerService
{
    public SingleRoundMakerService(ILogger<SingleRoundMakerService> log)
    {
        this.log = log;
    }

    public async Task MakeRound(CancellationToken cancellationToken)
    {
        log.LogInformation("Making round");
    }

    private readonly ILogger<SingleRoundMakerService> log;
}