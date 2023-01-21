using RandomCoffeeServer.Domain.Services.Coffee.Rounds;

namespace RandomCoffeeServer.Domain.Hosting;

public class RoundsMakerBackgroundService : BackgroundService
{
    public RoundsMakerBackgroundService(MockRoundsService roundsService)
    {
        this.roundsService = roundsService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await roundsService.Run(stoppingToken);
    }

    private readonly MockRoundsService roundsService;
}