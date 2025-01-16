namespace IOweYou.Web.Repositories.Mail;

public interface IMailRepository
{
    void SendPasswortResetMail(Models.User user);
}