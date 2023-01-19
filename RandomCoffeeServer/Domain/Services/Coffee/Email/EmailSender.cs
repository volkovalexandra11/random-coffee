using System.Net.Mail;
using System.Text;
using RandomCoffeeServer.Domain.Models;

namespace RandomCoffeeServer.Domain.Services.Coffee.Email;

public class EmailSender
{
    private readonly SmtpClient smtpClient;
    private readonly MailAddress coffeeMailAddress;

    public EmailSender(
        SmtpClient smtpClient,
        EmailSettings emailSettings)
    {
        this.smtpClient = smtpClient;
        this.coffeeMailAddress = new MailAddress(
            emailSettings.CoffeeMailAddress,
            emailSettings.CoffeeMailDisplayName
        );
    }

    public async Task SendMessage(CoffeeMailMessage coffeeMailMessage, User toUser)
    {
        var mailMessage = new MailMessage(
            from: coffeeMailAddress,
            to: new MailAddress(toUser.Email, toUser.FullName)
        )
        {
            SubjectEncoding = Encoding.UTF8,
            BodyEncoding = Encoding.UTF8,
            ReplyToList = { coffeeMailAddress },

            Subject = coffeeMailMessage.Subject,
            Body = coffeeMailMessage.Body
        };

        await smtpClient.SendMailAsync(mailMessage);
    }
}