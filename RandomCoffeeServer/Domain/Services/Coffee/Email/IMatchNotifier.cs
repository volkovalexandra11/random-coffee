using RandomCoffeeServer.Domain.Models;

namespace RandomCoffeeServer.Domain.Services.Coffee.Email;

public interface IMatchNotifier
{
    Task Notify(Group group, User toUser, User? matchedUser);
}