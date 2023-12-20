namespace LokataAdministrative2.Models
{
    public class NotificationDto : BaseDto
    {
        public string Message { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Email { get; set; } = string.Empty;
    }
}