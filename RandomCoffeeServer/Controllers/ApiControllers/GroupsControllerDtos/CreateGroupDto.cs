namespace RandomCoffeeServer.Controllers.ApiControllers.GroupsControllerDtos;

public class CreateGroupDto
{
    public string Name { get; set; }
    public bool IsPrivate { get; set; }
    public DateTime NextRoundDate { get; set; }
    public int IntervalDays { get; set; }
    public string? GroupPictureUrl { get; set; }
    public string? GroupDescription { get; set; }
}