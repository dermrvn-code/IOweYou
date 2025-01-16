using IOweYou.Web.Repositories.APIs;

namespace IOweYou.Web.Services.APIs;

public class QrCodeService : IQrCodeService
{
    private readonly IQrCodeRepository _qrCodeRepository;

    public QrCodeService(IQrCodeRepository qrCodeRepository)
    {
        _qrCodeRepository = qrCodeRepository;
    }

    public async Task<Stream?> GetQrCodeForUser(Models.User user)
    {
        return await _qrCodeRepository.GetQrCodeForUser(user);
    }
}