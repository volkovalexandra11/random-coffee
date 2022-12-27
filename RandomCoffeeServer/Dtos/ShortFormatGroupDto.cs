namespace RandomCoffeeServer.Controllers.GroupsControllerDtos;

public class ShortFormatGroupDto
{
    public Guid GroupId { get; }
    public string Name { get; }
    public int ParticipantsCount { get; }

    public ShortFormatGroupDto(Guid groupId, string name, int? participantsCount)
    {
        GroupId = groupId;
        Name = name;
        ParticipantsCount = participantsCount ?? 0;
    }
}