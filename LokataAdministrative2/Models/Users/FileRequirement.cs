namespace LokataAdministrative2.Models.Users
{
    public class FileRequirement
    {
        public string FileName { get; set; } = string.Empty;
        public string FileUrl { get; set; } = string.Empty;
        public bool IsApproved { get; set; }
        public bool IsDeclined { get; set; }
    }
}