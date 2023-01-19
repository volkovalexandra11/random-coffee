namespace RandomCoffeeServer.Domain.Services.Coffee.UserMatching;

public class RandomUserMatcher : IUserMatcher
{
    private readonly Random random;

    public RandomUserMatcher()
    {
        this.random = new Random();
    }

    public IEnumerable<(Guid userId1, Guid? userId2)> GetUserMatches(Guid[] userIds)
    {
        var shuffledIds = userIds.Shuffled(random);
        for (var i = 0; i < shuffledIds.Count; i += 2)
        {
            yield return (shuffledIds[i], shuffledIds[i + 1]);
        }
        if (shuffledIds.Count % 2 != 0)
        {
            yield return (shuffledIds.Last(), null);
        }
    }
}