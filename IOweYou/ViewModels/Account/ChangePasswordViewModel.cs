using System.ComponentModel.DataAnnotations;

namespace IOweYou.ViewModels.Account;

public class ChangePasswordViewModel
{
    public string Token { get; set; }
    public bool UseUserToken { get; set; }
    
    
    
    [Required(ErrorMessage = "Please enter your old password")]
    [DataType(DataType.Password)]
    public string OldPassword { get; set; }
    
    [Required(ErrorMessage = "Please enter your new password")]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; }
    
    [Required(ErrorMessage = "Please confirm your new password")]
    [Compare(nameof(NewPassword), ErrorMessage="Passwords do not match.")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; }
}