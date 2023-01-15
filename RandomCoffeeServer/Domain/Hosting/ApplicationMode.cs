namespace RandomCoffeeServer.Domain.Hosting;

[Flags]
public enum ApplicationMode
{
    None = 0,

    ApiServer = 1,

    DbUpdaterOnRequest = 2 << 0,
    RoundsMakerOnRequest = 2 << 1,

    DbUpdateOnStartup = 2 << 2,
    RoundsMakerHostedService = 2 << 3
}