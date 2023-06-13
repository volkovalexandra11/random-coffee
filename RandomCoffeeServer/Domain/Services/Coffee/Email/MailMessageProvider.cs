using RandomCoffeeServer.Domain.Models;

namespace RandomCoffeeServer.Domain.Services.Coffee.Email;

public class MailMessageProvider
{
    public CoffeeMailMessage GetMessage(Group group, User toUser, User? matchedUser)
    {
        return matchedUser != null
            ? GetHasMatchMessage(group, toUser, matchedUser)
            : GetNoMatchMessage(group, toUser);
    }

    private CoffeeMailMessage GetNoMatchMessage(Group group, User toUser)
    {
        return new CoffeeMailMessage
        {
            Subject = $"Новый раунд случайного кофе в группе {group.Name}",
            Body = $"Привет, {toUser.FullName}!\n" +
                   "Извини, в этом раунде мы не нашли тебе собеседника"
        };
    }

    private CoffeeMailMessage GetHasMatchMessage(Group group, User toUser, User matchedUser)
    {
        return new CoffeeMailMessage
        {
            Subject = $"Новый раунд случайного кофе в группе {group.Name}",
            Body = $"Привет, {toUser.FullName}!\n" +
                   $"Твой собеседник в этом раунде {matchedUser.FullName}\n" +
                   $"Можешь написать ему на почту: {matchedUser.Email}\n" +
                   $"Или в telegram: {matchedUser.Telegram}"
        };
    }
}