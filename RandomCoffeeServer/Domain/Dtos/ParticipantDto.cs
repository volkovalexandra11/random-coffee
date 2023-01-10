namespace RandomCoffeeServer.Domain.Dtos;

public class ParticipantDto
{
    public Guid UserId { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string? ProfilePictureUrl { get; init; }
}