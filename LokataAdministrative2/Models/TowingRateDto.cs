using System.ComponentModel.DataAnnotations;

namespace LokataAdministrative2.Models
{
    public class TowingRateDto : BaseDto
    {
        [Required(ErrorMessage = "Load vehicle should not be empty.")]
        public string VehicleType { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        [Required(ErrorMessage = "Fee should not be empty.")]
        public double Fee { get; set; }
    }
}