namespace RandomCoffeeServer.Domain.Services.Coffee.Rounds;

// Периодически проводит раунды в созревших группах
public class MockRoundsService
{
    public MockRoundsService(
        ScheduledRoundsMakerService roundsMakerService,
        DevelopmentPeriodicRoundsSettings checkSettings,
        ILogger<MockRoundsService> log)
    {
        if (checkSettings?.CheckEverySeconds is null)
            throw new ArgumentException($"Expected {nameof(DevelopmentPeriodicRoundsSettings)} configuration");

        this.log = log;
        this.checkSettings = TimeSpan.FromSeconds(checkSettings.CheckEverySeconds);
        this.roundsMakerService = roundsMakerService;
    }

    public async Task Run(CancellationToken cancellationToken)
    {
        log.LogInformation("Starting timed rounds service");
        var timer = new PeriodicTimer(checkSettings);

        while (await timer.WaitForNextTickAsync(cancellationToken))
        {
            log.LogInformation("Callings rounds service with timer");
            await roundsMakerService.MakeRound(cancellationToken);
        }
    }

    private readonly ScheduledRoundsMakerService roundsMakerService;
    private readonly TimeSpan checkSettings;
    private readonly ILogger<MockRoundsService> log;
}