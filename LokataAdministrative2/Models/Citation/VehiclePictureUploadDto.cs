namespace LokataAdministrative2.Models.Citation
{
    public class VehiclePictureUploadDto
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? PlateNo { get; set; }
        public List<byte[]> Pictures { get; set; } = new();
        public List<string> FileNames { get; set; } = new();
    }
}