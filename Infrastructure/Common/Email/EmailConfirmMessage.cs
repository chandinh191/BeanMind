namespace Infrastructure.Common.Email;

public class EmailConfirmMessage
{
    public string ToAddress { get; private set; }

    public EmailConfirmMessage Create(string toAddress)
    {
        _ = toAddress ?? throw new ArgumentNullException("Address need to be provided");
        return new EmailConfirmMessage { ToAddress = toAddress };
    }
}
