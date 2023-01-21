using System.Net.Mail;
using System.Text;
using Newtonsoft.Json;
using RandomCoffeeServer.Domain.Models;

namespace RandomCoffeeServer.Domain.Services.Coffee.Email;

public class EmailSender
{
    private readonly SmtpClient smtpClient;
    private readonly MailAddress coffeeMailAddress;
    private readonly ILogger<EmailSender> log;

    public EmailSender(
        SmtpClient smtpClient,
        EmailSettings emailSettings,
        ILogger<EmailSender> log)
    {
        this.smtpClient = smtpClient;
        this.coffeeMailAddress = new MailAddress(
            emailSettings.CoffeeMailAddress,
            emailSettings.CoffeeMailDisplayName
        );
        this.log = log;
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

        try
        {
            await smtpClient.SendMailAsync(mailMessage);
        }
        catch (Exception e) when (e is SmtpException)
        {
            log.LogError($"Couldn't send message to {JsonConvert.SerializeObject(toUser)}.\n{e}");
        }
    }
}