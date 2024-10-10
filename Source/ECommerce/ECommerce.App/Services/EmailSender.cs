namespace ECommerce.App.Services;

using System.Net;
using System.Net.Mail;
using ECommerce.App.Database.Entities.Security;
using Microsoft.AspNetCore.Identity;

public class EmailSender : IEmailSender<User>
{
    public Task SendConfirmationLinkAsync(User user, string email, string confirmationLink)
    {
        throw new NotImplementedException();
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        SmtpClient client = new SmtpClient
        {
            Port = 587,
            Host = "smtp.mailersend.net",
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential("MS_kJAD6V@trial-3vz9dlep5dq4kj50.mlsender.net", "hE5zow0GmkzfE7uE"),

        };

        await client.SendMailAsync("damiansimanuk@gmail.com", email, subject, htmlMessage);
    }

    public async Task SendPasswordResetCodeAsync(User user, string email, string resetCode)
    {
        await SendEmailAsync(email, "Reset your password", $"Please reset your password using the following code: {resetCode}");
    }

    public Task SendPasswordResetLinkAsync(User user, string email, string resetLink)
    {
        throw new NotImplementedException();
    }
}