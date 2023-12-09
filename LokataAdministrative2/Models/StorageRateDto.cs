namespace LokataAdministrative2.Models
{
    public class StorageRateDto : BaseDto
    {
        public string VehicleType { get; set; } = string.Empty;
        public double Fee { get; set; }
    }
}