using LokataAdministrative2.Models.Users;
using System.Net.Http.Json;

namespace LokataAdministrative2.Services.AdminClient
{
    public class AdminAuthClient
    {
        private readonly HttpClient _adminAuthClient;

        public AdminAuthClient(HttpClient adminAuthClient)
        {
            _adminAuthClient = adminAuthClient;
        }

        public async Task<string> LoginPostRequest(AdminLogin dto)
        {
            var response = await _adminAuthClient.PostAsJsonAsync("api/adminauth/signin", dto);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> SignupPostRequest(AdminDto dto)
        {
            var response = await _adminAuthClient.PostAsJsonAsync("api/adminauth/signup", dto);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
