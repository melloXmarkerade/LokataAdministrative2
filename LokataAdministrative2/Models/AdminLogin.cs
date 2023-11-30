using System.ComponentModel.DataAnnotations;

namespace LokataAdministrative2.Models
{
    public class AdminLogin
    {
        [Required(ErrorMessage = "Username should not be empty.")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password should not be empty.")]
        public string Password { get; set; } = string.Empty;
    }
}