namespace LokataAdministrative2.Models
{
    public class Requirement
    {
        public string? FileName { get; set; }
        public string? FileUrl { get; set; }
        public bool IsApproved { get; set; }
        public bool IsDeclined { get; set; }
    }
}