using System.ComponentModel.DataAnnotations;

namespace LokataAdministrative2.Models
{
    public class AdminSignup : BaseDto
    {

        [Required(ErrorMessage = "Username should not be empty.")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password should not be empty.")]
        public string? Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Firstname should not be empty.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Lastname should not be empty.")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Id should not be empty.")]
        public string GovernmentId { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public bool IsApproved { get; set; }

        public string DateCreated { get; set; } =string.Empty;
    }
}