using System.ComponentModel.DataAnnotations;
using IOweYou.Models.Shared;

namespace IOweYou.ViewModels.Home;

public class SendViewModel
{
    [Required(ErrorMessage = "Please enter a username")]
    public string UserToSendTo { get; set; }
    
    [Required(ErrorMessage = "Please enter the value")]
    [DataType(DataType.Currency)]
    public double Value { get; set; }
}