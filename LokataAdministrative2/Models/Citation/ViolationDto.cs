using LokataAdministrative2.Models.Citation;
using System.ComponentModel.DataAnnotations;

namespace LokataAdministrative2.Models
{
    public class ViolationDto : BaseDto
    {
        [Required(ErrorMessage = "Category should not be empty.")]
        public string Category { get; set; } = string.Empty;

        [Required(ErrorMessage = "Name should not be empty.")]
        public string Name { get; set; } = string.Empty;
        public ViolationFeeDto ViolationFees { get; set; } = new();
    }
}
