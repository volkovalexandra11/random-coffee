namespace RandomCoffeeServer.Dtos;

public class UserRoundDto
{
    public Guid UserId { get; init; }
    public Guid RoundId { get; init; }
    public Guid MatchUserId { get; init; }
}