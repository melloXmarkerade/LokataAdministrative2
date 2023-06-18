namespace LokataAdministrative2.Models
{
    public class UserRequirement : BaseDto
    {
        public string? LicenseNo { get; set; }

        public string? Email { get; set; }

        public string? DateSubmitted { get; set; }

        public List<Requirement>? Requirements { get; set; }
    }
}
