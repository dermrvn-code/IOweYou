﻿using System.ComponentModel.DataAnnotations;

namespace IOweYou.ViewModels.Account;

public class LoginViewModel
{
    [Required(ErrorMessage = "Please enter your username")]
    public string Username { get; set; }
    
    [Required(ErrorMessage = "Please enter your password")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}