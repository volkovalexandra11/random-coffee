using System.Net;
using System.Net.Mail;
using RandomCoffeeServer.Domain.Models;

namespace RandomCoffeeServer.Domain.Services.Coffee;

public class EmailService
{

    public void SendMessage(string groupName, User toUser, User? pairUser)
    {
        SmtpClient mySmtpClient = new SmtpClient("smtp.mail.ru", 587);
        mySmtpClient.EnableSsl = true;

        NetworkCredential basicAuthenticationInfo = new
            NetworkCredential("randomcoffee@mail.ru", Environment.GetEnvironmentVariable("EMAIL_PASS"));
        mySmtpClient.Credentials = basicAuthenticationInfo;

        // add from,to mailaddresses
        MailAddress from = new MailAddress("randomcoffee@mail.ru", "Random coffee");
        MailAddress to = new MailAddress(toUser.Email, toUser.FullName);
        MailMessage myMail = new MailMessage(from, to);

        // add ReplyTo
        MailAddress replyTo = new MailAddress("randomcoffee@mail.ru");
        myMail.ReplyToList.Add(replyTo);

        if (pairUser is not null)
        {
            // set subject and encoding
            myMail.Subject = $"Новый раунд случайного кофе в группе {groupName}";
            myMail.SubjectEncoding = System.Text.Encoding.UTF8;

            // set body-message and encoding
        
            // это если захочется отправлять HTML
            // myMail.Body = "<b>Test Mail</b><br>using <b>HTML</b>.";
            // myMail.IsBodyHtml = true;
        
            myMail.Body = $"Привет, {toUser.FullName}!\n" +
                          $"Твой собеседник в этом раунде {pairUser.FullName}\n" +
                          $"Можешь написать ему на почту: {pairUser.Email}";
            myMail.BodyEncoding = System.Text.Encoding.UTF8;
        }
        else
        {
            // set subject and encoding
            myMail.Subject = $"Новый раунд случайного кофе в группе {groupName}";
            myMail.SubjectEncoding = System.Text.Encoding.UTF8;

            // set body-message and encoding

            myMail.Body = $"Привет, {toUser.FullName}!\n" +
                          $"Извини, в этом раунде мы не нашли тебе собеседника";
            myMail.BodyEncoding = System.Text.Encoding.UTF8;
        }
        
        mySmtpClient.Send(myMail);

    }

}