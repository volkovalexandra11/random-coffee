using RandomCoffeeServer.Dtos;

namespace RandomCoffeeServer.Controllers.GroupsControllerDtos;

public class GetGroupDto
{
    public Guid GroupId { get; }
    public Guid AdminUserId { get; }
    public string Name { get; }
    public bool IsPrivate { get; }
    public IEnumerable<GetGroupUserDto> Users { get; }

    public GetGroupDto(GroupDto groupDto, IEnumerable<UserDto> users)
    {
        GroupId = groupDto.GroupId;
        AdminUserId = groupDto.AdminUserId;
        Name = groupDto.Name;
        IsPrivate = groupDto.IsPrivate;
        Users = users.Select(user => new GetGroupUserDto(user));
    }
}