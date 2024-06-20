namespace Infrastructure.Common.Email;

public class SmtpSettings
{
    public required string Server { get; set; }
    public required int Port { get; set; }
    public required string SenderName { get; set; }
    public required string SenderEmail { get; set; }
    public required string UserName { get; set; }
    public required string Password { get; set; }
}
