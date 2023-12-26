namespace LokataAdministrative2.Models.Users
{
    public class UserReceipt : BaseDto
    {
        public string TctNo { get; set; } = string.Empty;
        public string LicenseNo { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string DateSubmitted { get; set; } = string.Empty;
        public string PlateNo { get; set; } = string.Empty;
        public FileRequirement? Receipt { get; set; }
    }
}