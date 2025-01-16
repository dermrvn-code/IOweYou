using System.Text;
using System.Text.Json;
using IOweYou.Web.Services.User;

namespace IOweYou.Web.Repositories.APIs;

public class QrCodeRepository : IQrCodeRepository
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<QrCodeRepository> _logger;
    private readonly IUserService _userService;

    public QrCodeRepository(ILogger<QrCodeRepository> logger, HttpClient httpClient, IUserService userService,
        IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _httpClient = httpClient;
        _userService = userService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Stream?> GetQrCodeForUser(Models.User user)
    {
        var apiKey = Environment.GetEnvironmentVariable("QR-CODE-API-KEY");
        if (apiKey == null) return null;

        var request = _httpContextAccessor.HttpContext?.Request;
        if (request == null)
            throw new InvalidOperationException("HttpContext is not available");

        var domain = $"{request.Scheme}://{request.Host}";


        var payload = new
        {
            frame_name = "no-frame",
            qr_code_text = domain + "/user/" + user.Username,
            image_format = "PNG",
            image_width = 300,
            foreground_color = "#6c757d",
            marker_right_inner_color = "#6c757d",
            marker_right_outer_color = "#0d6efd",
            marker_left_inner_color = "#6c757d",
            marker_left_outer_color = "#0d6efd",
            marker_bottom_inner_color = "#6c757d",
            marker_bottom_outer_color = "#0d6efd",
            marker_left_template = "version8",
            marker_right_template = "version7",
            marker_bottom_template = "version6"
        };
        var endpoint = "https://api.qr-code-generator.com/v1/create?access-token=" + apiKey;
        var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(endpoint, content);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStreamAsync();
    }
}