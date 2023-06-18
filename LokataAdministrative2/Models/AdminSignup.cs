namespace LokataAdministrative2.Models
{
    public class AdminSignup : BaseDto
    {
        public string GovernmentId { get; set; } = string.Empty;
        public string? Password { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string PhoneNo { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
    }
}
