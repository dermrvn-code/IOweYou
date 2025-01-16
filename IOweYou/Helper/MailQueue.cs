using MimeKit;

namespace IOweYou.Helper;

public class MailQueue
{
    private Queue<MimeMessage> _messages = new Queue<MimeMessage>();

    public MailQueue()
    {
    }
    
    public void Enqueue(MimeMessage message)
    {
        message.From.Add(new MailboxAddress("IOU", Environment.GetEnvironmentVariable("FROMMAILADDRESS")));
        _messages.Enqueue(message);
    }

    public MimeMessage Dequeue()
    {
        return _messages.Dequeue();
    }

    public bool HasMessages
    {
        get { return _messages.Count > 0; } 
    }
}