using System.ComponentModel.DataAnnotations;

namespace IOweYou.ViewModels.Account;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Please enter your email address")]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Please provide a username")]
    [MinLength(6, ErrorMessage="The Username must be at least 6 characters long.")]
    public string Username { get; set; }
    
    [Required(ErrorMessage = "Please provide a password")]
    [MinLength(5, ErrorMessage="The Password must be at least 6 characters long.")]
    [Compare(nameof(ConfirmPassword), ErrorMessage="Passwords do not match.")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    
    [Required(ErrorMessage = "Please confirm your password")]
    [Compare(nameof(Password), ErrorMessage="Passwords do not match.")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; }
}