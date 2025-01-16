using IOweYou.Helper;
using IOweYou.Models;
using IOweYou.Web.Repositories.User;
using MimeKit;

namespace IOweYou.Web.Repositories.Mail;

public class MailRepository : IMailRepository
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly ILogger<MailRepository> _logger;
    private readonly MailQueue _mailQueue;
    private readonly IUserRepository _userRepository;

    public MailRepository(ILogger<MailRepository> logger, IUserRepository userRepository, MailQueue mailQueue,
        IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _userRepository = userRepository;
        _mailQueue = mailQueue;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<bool> SendPasswortResetMail(Models.User user)
    {
        var text = "You've requested to reset your password!";
        var link = "/changepassword/{{token}}";

        return await SendMail(user, "Reset Password", text, "Change password", link);
    }

    public async Task<bool> SendRegistrationMail(Models.User user)
    {
        var text = "You've registered to IOU! Please verify your account.";
        var link = "/verifyaccount/{{token}}";

        return await SendMail(user, "Registration to IOU", text, "Verify account", link, hoursTillExpire: 72);
    }

    public async Task<bool> SendChangeAdressMail(Models.User user, string newmail)
    {
        var hashedMail = Hasher.UrlSecureHashValue(newmail);
        var text = "You've requested to change your email! Please verify it.";
        var link = "/validatechangemail/{{token}}?newmail=" + newmail + "&hash=" + hashedMail;

        return await SendMail(user, "Change your email", text, "Change email", link, newmail, 24);
    }

    private string CreateHTMLMail(string title, string text, string calltoaction, string link)
    {
        // Load the HTML template
        var htmlTemplatePath = "MailTemplate/Mail.html";
        var logoPath = "wwwroot/Logo.png";
        var htmlContent = File.ReadAllText(htmlTemplatePath);

        var imageBytes = File.ReadAllBytes(logoPath);
        var base64String = Convert.ToBase64String(imageBytes);

        // Replace placeholders with variable data
        htmlContent = htmlContent.Replace("{{title}}", title)
            .Replace("{{text}}", text)
            .Replace("{{calltoaction}}", calltoaction)
            .Replace("{{link}}", link)
            .Replace("{{image_base64}}", base64String);

        return htmlContent;
    }

    public async Task<bool> SendMail(Models.User user, string title, string text, string calltoaction, string link,
        string? email = null, int hoursTillExpire = 1)
    {
        var request = _httpContextAccessor.HttpContext?.Request;
        if (request == null)
            throw new InvalidOperationException("HttpContext is not available");

        var token = new UserToken(user, hoursTillExpire);

        var success = await _userRepository.AddToken(token);
        if (!success) return false;


        link = $"{request.Scheme}://{request.Host}" + link;

        var linkWithToken = link.Replace("{{token}}", token.ID.ToString());
        var bodyText = $"Hello {user.Username}!<br><br>{text}";

        var address = email == null ? user.Email : email;
        var message = new MimeMessage();
        message.To.Add(new MailboxAddress("", address));
        message.Subject = "IOU - " + title;
        message.Body = new TextPart("html")
        {
            Text = CreateHTMLMail(title, bodyText, calltoaction, linkWithToken)
        };
        _mailQueue.Enqueue(message);
        _logger.LogInformation("Queued mail to " + address);
        return true;
    }
}