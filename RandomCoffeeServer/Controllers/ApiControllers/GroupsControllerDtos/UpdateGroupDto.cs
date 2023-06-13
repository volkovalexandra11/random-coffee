namespace RandomCoffeeServer.Controllers.ApiControllers.GroupsControllerDtos;

public class UpdateGroupDto
{
    public string Name { get; init; }
    public string Tag { get; init; }
    public bool IsPrivate { get; init; }
    public string? GroupPictureUrl { get; init; }
    public string? GroupDescription { get; init; }
}