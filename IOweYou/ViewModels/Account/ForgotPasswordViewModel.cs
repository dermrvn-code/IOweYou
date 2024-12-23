using System.ComponentModel.DataAnnotations;

namespace IOweYou.ViewModels;

public class ForgotPasswordViewModel
{
    [Required(ErrorMessage = "Please enter your email address")]
    [EmailAddress]
    public string Email { get; set; }
}