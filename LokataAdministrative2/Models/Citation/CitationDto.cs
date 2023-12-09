using LokataAdministrative2.Models.Citation;

namespace LokataAdministrative2.Models
{
    public class CitationDto : BaseDto
    {
        public string? TctNo { get; set; }
        public string? LicenseNo { get; set; } 
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LicenseType { get; set; }
        public string? DateRecord { get; set; }
        public AddressDto? Address { get; set; }
        public VehicleDto? VehicleDescription { get; set; }
        public PlaceDto? PlaceApprehended { get; set; }
        public List<UserViolationDto>? Violations { get; set; }
        public OfficerDto? ApprehendingOfficer { get; set; }
        public List<string>? ItemConfiscated { get; set; }
        public PaymentSummaryDto? PaymentSummary { get; set; }
    }
}
