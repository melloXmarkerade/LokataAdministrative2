namespace LokataAdministrative2.Models
{
    public class UserViolationDto
    {
        public string? Category { get; set; }
        public string? Name { get; set; }
        public string? Offense { get; set; }
        public double Fine { get; set; }
    }
}