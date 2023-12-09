namespace LokataAdministrative2.Models.Citation
{
    public class TrackingDto : BaseDto
    {
        public string ImpoundingArea { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? Longitude { get; set; }
        public string? Latitude { get; set; }
    }
}
