using RandomCoffeeServer.Domain.Models;

namespace RandomCoffeeServer.Domain.Services.Coffee.Email;

public class EmailMatchNotifier : IMatchNotifier
{
    private readonly MailMessageProvider messageProvider;
    private readonly EmailSender emailSender;

    public EmailMatchNotifier(
        MailMessageProvider messageProvider,
        EmailSender emailSender)
    {
        this.messageProvider = messageProvider;
        this.emailSender = emailSender;
    }

    public async Task Notify(Group group, User toUser, User? matchedUser)
    {
        var notificationMessage = messageProvider.GetMessage(group, toUser, matchedUser);
        await emailSender.SendMessage(notificationMessage, toUser);
    }
}