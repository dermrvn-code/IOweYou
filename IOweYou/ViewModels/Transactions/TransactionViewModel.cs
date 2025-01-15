using System.ComponentModel.DataAnnotations;

namespace IOweYou.ViewModels.Transactions;

public class TransactionViewModel
{
    [Required(ErrorMessage = "Please enter a username")]
    public string UserToSendTo { get; set; }
    
    [Required(ErrorMessage = "Please enter a currency")]
    public string Currency  { get; set; }
    
    [Required(ErrorMessage = "Please enter the value")]
    public double Value { get; set; }
}