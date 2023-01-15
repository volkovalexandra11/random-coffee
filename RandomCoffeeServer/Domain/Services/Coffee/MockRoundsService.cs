namespace RandomCoffeeServer.Domain.Services.Coffee;

public class MockRoundsService
{
    public MockRoundsService(SingleRoundMakerService roundMakerService, TimeSpan checkPeriod,
        ILogger<MockRoundsService> log)
    {
        this.log = log;
        this.checkPeriod = checkPeriod;
        this.roundMakerService = roundMakerService;
    }

    public async Task Run(CancellationToken cancellationToken)
    {
        log.LogInformation("Starting timed rounds service");
        var timer = new PeriodicTimer(checkPeriod);

        while (await timer.WaitForNextTickAsync(cancellationToken))
        {
            log.LogInformation("Callings rounds service with timer");
            await roundMakerService.MakeRound(cancellationToken);
        }
    }

    private readonly SingleRoundMakerService roundMakerService;
    private readonly TimeSpan checkPeriod;
    private readonly ILogger<MockRoundsService> log;
}