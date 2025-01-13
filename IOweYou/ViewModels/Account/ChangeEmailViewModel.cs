using System.ComponentModel.DataAnnotations;

namespace IOweYou.ViewModels.Account;

public class ChangeEmailViewModel
{
    [Required(ErrorMessage = "Please enter your username")]
    public string Email { get; set; }
}