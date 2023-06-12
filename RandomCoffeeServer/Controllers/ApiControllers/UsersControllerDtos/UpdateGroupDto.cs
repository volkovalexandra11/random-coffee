namespace RandomCoffeeServer.Controllers.ApiControllers.UsersControllerDtos;

public class UpdateGroupDto
{
    public string Name { get; init; }
    public bool IsPrivate { get; init; }
    public string? GroupPictureUrl { get; init; }
    public string? GroupDescription { get; init; }
}