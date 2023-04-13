namespace LokataAdministrative2.Models
{
    public class CitationDTO
    {
        public string? Id { get; set; }
        public string? TctNo { get; set; }
        public string? LicesnseNo { get; set; } 
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LicenseType { get; set; }
        public string? Officer { get; set; }
        public AddressDto? Address { get; set; }
        public VehicleDto? VehicleDescription { get; set; }
        public PlaceDto? PlaceApprehended { get; set; }
        public List<ViolationDto>? Violations { get; set; }


    }
}
