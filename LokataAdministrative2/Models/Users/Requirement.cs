using System.ComponentModel.DataAnnotations;

namespace LokataAdministrative2.Models.Users
{
    public class Requirement : BaseDto
    {
        [Required(ErrorMessage = "Name should not be empty.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Description should not be empty.")]
        public string? Description { get; set; }
    }
}