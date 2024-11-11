using LokataAdministrative2.Models.Citation;

namespace LokataAdministrative2.Models
{
    public class VehicleDto : BaseDto
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? TctNo { get; set; }
        public string? LicenseNo { get; set; }
        public string? PlateNo { get; set; }
        public ImpoundedAreaDto? Location { get; set; }
        public string? Make { get; set; }
        public string? Model { get; set; }
        public string? Color { get; set; }
        public bool IsImpounded { get; set; }
        public string? DateImpounded { get; set; }
        public string? Status { get; set; }
        public List<VehiclePictureDto> Pictures { get; set; } = new();
    }
}
