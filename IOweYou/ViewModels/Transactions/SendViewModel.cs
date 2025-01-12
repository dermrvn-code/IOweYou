using System.ComponentModel.DataAnnotations;

namespace IOweYou.ViewModels.Transactions;

public class SendViewModel
{
    [Required(ErrorMessage = "Please enter a username")]
    public string UserToSendTo { get; set; }
    
    [Required(ErrorMessage = "Please enter a currency")]
    public string Currency  { get; set; }
    
    [Required(ErrorMessage = "Please enter the value")]
    [DataType(DataType.Currency)]
    public double Value { get; set; }
}