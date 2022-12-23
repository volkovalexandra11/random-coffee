using RandomCoffeeServer.Dtos;

namespace RandomCoffeeServer.Controllers.GroupsControllerDtos;

public class GroupWithParticipantsDto
{
    public Guid GroupId { get; }
    public Guid AdminUserId { get; }
    public string Name { get; }
    public bool IsPrivate { get; }
    public List<ParticipantDto> Participants { get; }

    public GroupWithParticipantsDto(GroupDto groupDto, IEnumerable<ParticipantDto> participants)
    {
        GroupId = groupDto.GroupId;
        AdminUserId = groupDto.AdminUserId;
        Name = groupDto.Name;
        IsPrivate = groupDto.IsPrivate;
        Participants = participants.ToList();
    }
}