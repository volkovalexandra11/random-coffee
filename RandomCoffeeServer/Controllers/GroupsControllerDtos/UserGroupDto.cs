using RandomCoffeeServer.Dtos;

namespace RandomCoffeeServer.Controllers.GroupsControllerDtos;

public class UserGroupDto
{
    public Guid GroupId { get; }
    public string Name { get; }
    public int UsersCount { get; }

    public UserGroupDto(GetGroupDto groupDto)
    {
        GroupId = groupDto.GroupId;
        Name = groupDto.Name;
        UsersCount = groupDto.Users.Count();
    }
}