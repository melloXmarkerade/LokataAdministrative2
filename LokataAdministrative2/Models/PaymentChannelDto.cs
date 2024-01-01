using System.ComponentModel.DataAnnotations;

namespace LokataAdministrative2.Models
{
    public class PaymentChannelDto : BaseDto
    {
        [Required(ErrorMessage = "Name should not be empty.")]
        public string? AccountName { get; set; }

        [Required(ErrorMessage = "Account No should not be empty.")]
        public string? AccountNo { get; set; }

        [Required(ErrorMessage = "BankName should not be empty.")]
        public string? BankName { get; set; }
    }
}
