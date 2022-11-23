namespace RandomCoffee.Dtos;

public class RoundDto
{
    public long RoundId { get; init; }
    public long GroupId { get; init; }
    public DateTime Date { get; init; }
    public bool WasNotifiedAbout { get; init; }
}