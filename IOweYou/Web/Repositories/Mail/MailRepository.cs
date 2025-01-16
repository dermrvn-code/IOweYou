using IOweYou.Helper;
using IOweYou.Web.Repositories.User;
using MimeKit;

namespace IOweYou.Web.Repositories.Mail;

public class MailRepository : IMailRepository
{
    
    private readonly ILogger<MailRepository> _logger;
    private readonly IUserRepository _userRepository;
    private readonly MailQueue _mailQueue;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public MailRepository(ILogger<MailRepository> logger, IUserRepository userRepository, MailQueue mailQueue, IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _userRepository = userRepository;
        _mailQueue = mailQueue;
        _httpContextAccessor = httpContextAccessor;
    }

    private string CreateHTMLMail(string title, string text, string calltoaction, string link)
    {
        // Load the HTML template
        string htmlTemplatePath = "MailTemplate/Mail.html";
        string logoPath = "wwwroot/Logo.png";
        string htmlContent = File.ReadAllText(htmlTemplatePath);
        
        byte[] imageBytes = File.ReadAllBytes(logoPath);
        string base64String = Convert.ToBase64String(imageBytes);

        // Replace placeholders with variable data
        htmlContent = htmlContent.Replace("{{title}}", title)
            .Replace("{{text}}", text)
            .Replace("{{calltoaction}}", calltoaction)
            .Replace("{{link}}", link)
            .Replace("{{image_base64}}", base64String);
        
        return htmlContent;
    }
    
    public void SendPasswortResetMail(Models.User user)
    {
        /*user.PasswordResetToken = passwordHelper.GenerateToken();
        userRepository.Update(user);*/
        
        var request = _httpContextAccessor.HttpContext?.Request;
        if (request == null)
            throw new InvalidOperationException("HttpContext is not available");
        
        
        string link = $"{request.Scheme}://{request.Host}/changepassword/";
        
        var message = new MimeMessage();
        message.To.Add(new MailboxAddress("", user.Email));
        message.Subject = "OOU - Reset Password";
        message.Body = new TextPart("html") 
        { 
            Text = CreateHTMLMail("Reset Password", $"Hello {user.Username}!<br><br>You've requested to reset your password!", "Change password", link)
        };
        _mailQueue.Enqueue(message);
    }

    /*public void SendRegistrationMail(User user)
    {
        
    }*/
}