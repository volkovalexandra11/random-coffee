namespace RandomCoffeeServer.Domain.Dtos;

public class ShortFormatGroupDto
{
    public Guid GroupId { get; init; }
    public string Name { get; init; }
    public int ParticipantsCount { get; init; }
    
    public DateTime NextRoundDate { get; init; }
}