namespace IOweYou.Web.Repositories.APIs;

public interface IQrCodeRepository
{
    Task<Stream?> GetQrCodeForUser(Models.User user);
}