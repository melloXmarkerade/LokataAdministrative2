namespace LokataAdministrative2.Models
{
    public class Requirement
    {
        public string FileName { get; set; } = string.Empty;
        public string FileUrl { get; set; } = string.Empty;
        public bool IsApproved { get; set; }
        public bool IsDeclined { get; set; }
    }
}