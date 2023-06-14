namespace RandomCoffeeServer.Controllers.ApiControllers.UsersControllerDtos;

public class UpdateUserDto
{
    public string Email { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string? ProfilePictureUrl { get; init; }
    public string? Telegram { get; init; }
}