namespace LokataAdministrative2.Models
{
    public class NotificationDto : BaseDto
    {
        public string Message { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}