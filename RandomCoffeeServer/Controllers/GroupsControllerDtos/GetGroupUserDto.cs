using RandomCoffeeServer.Dtos;

namespace RandomCoffeeServer.Controllers.GroupsControllerDtos;

public class GetGroupUserDto
{
    public Guid UserId { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string? ProfilePictureUrl { get; init; }

    public GetGroupUserDto(UserDto user)
    {
        UserId = user.UserId;
        FirstName = user.FirstName;
        LastName = user.LastName;
        ProfilePictureUrl = user.ProfilePictureUrl;
    }
}