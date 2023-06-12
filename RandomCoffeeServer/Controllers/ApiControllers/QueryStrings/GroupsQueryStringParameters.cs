namespace RandomCoffeeServer.Controllers.ApiControllers.QueryStrings;

public class GroupsQueryStringParameters : QueryStringParametersBase
{
    public Guid? UserId { get; set; }
    public Guid? AdminUserId { get; set; }
    public int? UsersCount { get; set; }
    public string? Name { get; set; }
    public string? Tag { get; set; }
}