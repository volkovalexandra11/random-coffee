namespace RandomCoffeeServer.Dtos;

public class RoundDto
{
    public Guid RoundId { get; init; }
    public Guid GroupId { get; init; }
    public DateTime Date { get; init; }
    public bool WasNotifiedAbout { get; init; }
}