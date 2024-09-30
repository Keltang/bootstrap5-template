namespace BlackBox.Domain.Models
{
    public class EmailMessage
    {
        public string Subject { get; }
        public string Body { get; }
        public string Recipient { get; }

        public EmailMessage(string subject, string body, string recipient)
        {
            Subject = subject;
            Body = body;
            Recipient = recipient;
        }
    }
}
