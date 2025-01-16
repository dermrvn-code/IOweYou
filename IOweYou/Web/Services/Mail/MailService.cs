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


    public async Task<bool> SendPasswortResetMail(Models.User user)
    {
        return await _mailRepository.SendPasswortResetMail(user);
    }

    public async Task<bool> SendRegistrationMail(Models.User user)
    {
        return await _mailRepository.SendRegistrationMail(user);
    }

    public async Task<bool> SendChangeAdressMail(Models.User user, string newmail)
    {
        return await _mailRepository.SendChangeAdressMail(user, newmail);
    }
}