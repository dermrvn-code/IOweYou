using IOweYou.Web.Repositories.Mail;

namespace IOweYou.Web.Services.Mail;

public class MailService : IMailService
{
    private readonly ILogger<MailService> _logger;
    private readonly IMailRepository _mailRepository;

    public MailService(ILogger<MailService> logger, IMailRepository mailRepository)
    {
        _logger = logger;
        _mailRepository = mailRepository;
    }


    public void SendPasswortResetMail(Models.User user)
    {
        _mailRepository.SendPasswortResetMail(user);
    }
}