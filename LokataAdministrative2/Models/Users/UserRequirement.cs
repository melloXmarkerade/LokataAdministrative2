namespace LokataAdministrative2.Models.Users
{
    public class UserRequirement : BaseDto
    {
        public string TctNo { get; set; } = string.Empty;
        public string LicenseNo { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string DateSubmitted { get; set; } = string.Empty;
        public string? PlateNo { get; set; } = string.Empty;
        public List<FileRequirement>? Requirements { get; set; }
    }
}
