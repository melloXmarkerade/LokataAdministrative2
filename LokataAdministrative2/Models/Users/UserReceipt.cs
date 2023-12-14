namespace LokataAdministrative2.Models.Users
{
    public class UserReceipt : BaseDto
    {
        public string LicenseNo { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string DateSubmitted { get; set; } = string.Empty;
        public string PlateNo { get; set; } = string.Empty;
        public Requirement? Receipt { get; set; }
    }
}