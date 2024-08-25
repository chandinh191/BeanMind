using Infrastructure.Common.Email;

namespace Infrastructure.Services;

public interface IEmailService
{
    Task SendEmailAsync(EmailMessage emailMessage);
    Task SendConfirmMailAsync(string email, string confirmationlink);
    Task SendInfoStudentAsync(string email, string username, string password);
    Task SendPasswordResetCodeAsync(string endpointHanler, string email, string resetCode);
    Task SendPasswordResetLinkAsync(string email, string resetLink);
}
