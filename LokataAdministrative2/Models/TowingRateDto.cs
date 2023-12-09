namespace LokataAdministrative2.Models
{
    public class TowingRateDto : BaseDto
    {
        public string VehicleType { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public double Fee { get; set; }
    }
}