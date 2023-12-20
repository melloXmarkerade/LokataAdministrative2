namespace LokataAdministrative2.Models
{
    public class UserDto : BaseDto
    {
        public string? LicenseNo { get; set; }

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? MiddleName { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }
    }
}
