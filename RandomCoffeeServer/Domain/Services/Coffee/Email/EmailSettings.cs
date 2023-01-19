namespace RandomCoffeeServer.Domain.Services.Coffee.Email;

public class EmailSettings
{
    public string SmtpServerEndpoint { get; set; }

    public string CoffeeMailAddress { get; set; }
    public string CoffeeMailDisplayName { get; set; }
}