namespace Infrastructure.Common.Email;

public class EmailAttachment
{
    public string Name { get; private set; }
    public byte[] Value { get; private set; }

    public static EmailAttachment Create(string name, byte[] value)
    {
        return new EmailAttachment
        {
            Name = name,
            Value = value
        };
    }
}
