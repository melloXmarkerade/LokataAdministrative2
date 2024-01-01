using System.ComponentModel.DataAnnotations;

namespace LokataAdministrative2.Models.Citation
{
    public class ImpoundedAreaDto : BaseDto
    {
        [Required(ErrorMessage = "Area should not be empty.")]
        public string ImpoundingArea { get; set; } = string.Empty;

        [Required(ErrorMessage = "Address should not be empty.")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Longitude should not be empty.")]
        public string? Longitude { get; set; }

        [Required(ErrorMessage = "Latitude should not be empty.")]
        public string? Latitude { get; set; }
    }
}
