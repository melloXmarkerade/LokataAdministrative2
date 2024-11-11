namespace LokataAdministrative2.Models.Users
{
    public class AdminLoginResponseDto
    {
        public AdminDto? Admin { get; set; }
        public string Token { get; set; } = string.Empty;
    }
}
