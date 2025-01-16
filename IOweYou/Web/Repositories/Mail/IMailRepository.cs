namespace IOweYou.Web.Repositories.Mail;

public interface IMailRepository
{
    Task<bool> SendPasswortResetMail(Models.User user);
    Task<bool> SendRegistrationMail(Models.User user);
    Task<bool> SendChangeAdressMail(Models.User user, string newmail);
}