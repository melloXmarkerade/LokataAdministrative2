using System.ComponentModel.DataAnnotations;

namespace LokataAdministrative2.Models
{
    public class StorageRateDto : BaseDto
    {
        [Required(ErrorMessage = "VehicleType should not be empty.")]
        public string VehicleType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Fee should not be empty.")]
        public double Fee { get; set; }
    }
}