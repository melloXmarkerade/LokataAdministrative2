using System.ComponentModel.DataAnnotations;

namespace LokataAdministrative2.Models.Citation
{
    public class ViolationFeeDto
    {
        [Required(ErrorMessage = "FirstOffense should not be empty.")]
        public double FirstOffense { get; set; }

        [Required(ErrorMessage = "SecondOffense should not be empty.")]
        public double SecondOffense { get; set; }

        [Required(ErrorMessage = "ThirdOffense should not be empty.")]
        public double ThirdOffense { get; set; }
    }
}