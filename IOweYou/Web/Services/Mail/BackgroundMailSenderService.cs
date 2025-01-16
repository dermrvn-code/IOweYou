using IOweYou.Helper;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace IOweYou.Web.Services.Mail;

public class BackgroundMailSenderService : BackgroundService
{
    private readonly ILogger<BackgroundMailSenderService> _logger;
    private readonly MailQueue _mailQueue;

    public BackgroundMailSenderService(ILogger<BackgroundMailSenderService> logger, MailQueue mailQueue)
    {
        _logger = logger;
        _mailQueue = mailQueue;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_mailQueue.HasMessages)
            {
                var message = _mailQueue.Dequeue();
                if (!SendMail(message, stoppingToken))
                    _mailQueue.Enqueue(message);
            }

            await Task.Delay(1000, stoppingToken);
        }
    }

    private bool SendMail(MimeMessage message, CancellationToken stoppingToken)
    {
        try
        {
            using (var client = new SmtpClient())
            {
                client.Connect(Environment.GetEnvironmentVariable("HOST"),
                    int.Parse(Environment.GetEnvironmentVariable("PORT") ?? "587"), SecureSocketOptions.Auto,
                    stoppingToken);
                client.Authenticate(Environment.GetEnvironmentVariable("USERNAME"),
                    Environment.GetEnvironmentVariable("PASSWORD"));
                client.Send(message);
                client.Disconnect(true);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Could not send mail!");
            return false;
        }

        return true;
    }
}