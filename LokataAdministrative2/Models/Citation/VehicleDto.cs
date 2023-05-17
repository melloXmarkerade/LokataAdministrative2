namespace LokataAdministrative2.Models
{
    public class VehicleDto : BaseDto
    {
        public string? PlateNo { get; set; }
        public string? Tracking { get; set; }
        public string? Make { get; set; }
        public string? Model { get; set; }
        public string? Color { get; set; }
    }
}
