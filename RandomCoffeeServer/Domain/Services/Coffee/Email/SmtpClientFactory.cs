using System.Net;
using System.Net.Mail;
using RandomCoffeeServer.Storage.YandexCloud.Lockbox;

namespace RandomCoffeeServer.Domain.Services.Coffee.Email;

public class SmtpClientFactory
{
    private readonly LockboxService lockboxService;
    private readonly EmailSettings emailSettings;

    public SmtpClientFactory(
        LockboxService lockboxService,
        EmailSettings emailSettings)
    {
        this.lockboxService = lockboxService;
        this.emailSettings = emailSettings;
    }

    public async Task<SmtpClient> Create()
    {
        var networkCredential = new NetworkCredential(
            emailSettings.CoffeeMailAddress,
            await lockboxService.GetCoffeeEmailPassword()
        );

        var (smtpHost, smtpPort) = GetEndpointData(emailSettings.SmtpServerEndpoint);
        return new SmtpClient(smtpHost, smtpPort)
        {
            Credentials = networkCredential,
            EnableSsl = true
        };
    }

    private static (string host, int port) GetEndpointData(string endpoint)
    {
        var endpointParts = endpoint.Split(':');
        return (endpointParts[0], int.Parse(endpointParts[1]));
    }
}