namespace RandomCoffeeServer.Domain.Hosting;

[Flags]
public enum ApplicationMode
{
    None = 0,
    ApiServer = 1,
    DatabaseUpdateJob = 2 << 0,
    RoundsMakerJob = 2 << 1,
    DatabaseUpdateHostedService = 2 << 2,
    RoundsMakerHostedService = 2 << 3
}