namespace RandomCoffeeServer.Domain.Services.Coffee.UserMatching;

public interface IUserMatcher
{
    IEnumerable<(Guid userId1, Guid? userId2)> GetUserMatches(Guid[] userIds);
}