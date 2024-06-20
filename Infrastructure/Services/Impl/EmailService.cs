using Infrastructure.Common.Email;
using MailKit.Net.Smtp;
using MimeKit;
using System.Text.Encodings.Web;

namespace Infrastructure.Services.Impl;

public class EmailService : IEmailService
{
    private readonly SmtpSettings _smtpSettings;

    public EmailService(SmtpSettings smtpSettings)
    {
        _smtpSettings = smtpSettings;
    }

    public async Task SendEmailAsync(EmailMessage emailMessage)
    {
        await SendAsync(CreateEmail(emailMessage));
    }

    private async Task SendAsync(MimeMessage message)
    {
        using var client = new SmtpClient();

        try
        {
            await client.ConnectAsync(_smtpSettings.Server, _smtpSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_smtpSettings.UserName, _smtpSettings.Password);
            await client.SendAsync(message);
        }
        catch
        {
            await client.DisconnectAsync(true);
            client.Dispose();

            throw;
        }
    }

    private MimeMessage CreateEmail(EmailMessage emailMessage)
    {
        // build body email
        var builder = new BodyBuilder { HtmlBody = emailMessage.Body };

        // add attachments to body builder
        if(emailMessage.Attachments.Count > 0)
        {
            foreach(var attachment in emailMessage.Attachments)
            {
                builder.Attachments.Add(attachment.Name, attachment.Value);
            }
        }

        // build MimeMessage
        var email = new MimeMessage
        {
            Subject = emailMessage.Subject,
            Body = builder.ToMessageBody(),
        };

        email.From.Add(new MailboxAddress(_smtpSettings.SenderName, _smtpSettings.SenderEmail));
        email.To.Add(new MailboxAddress(emailMessage.ToAddress.Split("@")[0], emailMessage.ToAddress));

        return email;
    }

    public async Task SendConfirmMailAsync(string email, string confirmationlink)
    {
        var subject = "Confirmation Your Email Address";
        var body = $"<p>Dear User,</p>\r\n    <p>Thank you for registering. Please click on the link below to confirm your email address and complete your registration:</p>\r\n    <a href=\"{HtmlEncoder.Default.Encode(confirmationlink)}\">Confirm Email</a>\r\n    <p>If you did not register for our service, please ignore this email.</p>\r\n    <p>Best,</p>\r\n    <p>RaeKyo Inc.</p>";
        await SendEmailAsync(EmailMessage.Create(email, body, subject));
    }

    public async Task SendPasswordResetCodeAsync(string endpointHanler, string email, string resetCode)
    {
        // build Url endpoint to handle password reset
        var passwordResetLink = $"{endpointHanler}?resetCode={resetCode}&email={email}";
        await SendPasswordResetLinkAsync(email, passwordResetLink);
    }

    public async Task SendPasswordResetLinkAsync(string email, string resetLink)
    {
        var subject = "Password Reset Request";
        var body = $"<p>Dear User,</p>\r\n    <p>We received a request to reset your password. If you did not make this request, please ignore this email.</p>\r\n    <p>Otherwise, please click on the link below to reset your password:</p>\r\n    <a href=\"{HtmlEncoder.Default.Encode(resetLink)}\">Reset Password</a>\r\n    <p>This link will expire in 24 hours.</p>\r\n    <p>Best,</p>\r\n    <p>RaeKyo Inc.</p>";
        await SendEmailAsync(EmailMessage.Create(email, body, subject));
    }
}
