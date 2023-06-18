namespace LokataAdministrative2.Models
{
    public class UserReceipt : BaseDto
    {
        public string? LicenseNo { get; set; }

        public string? Email { get; set; }

        public string? DateSubmitted { get; set; }

        public Requirement? Receipt { get; set; }
    }
}