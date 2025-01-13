using System.ComponentModel.DataAnnotations;

namespace IOweYou.ViewModels.Account;

public class ChangeUsernameViewModel
{
    [Required(ErrorMessage = "Please enter your username")]
    public string Username { get; set; }
}