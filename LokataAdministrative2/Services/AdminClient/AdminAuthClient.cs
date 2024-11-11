using LokataAdministrative2.Models.Users;
using System.Net.Http.Json;
using System.Text.Json;

namespace LokataAdministrative2.Services.AdminClient
{
    public class AdminAuthClient
    {
        private readonly HttpClient _adminAuthClient;

        public AdminAuthClient(HttpClient adminAuthClient)
        {
            _adminAuthClient = adminAuthClient;
        }

        public async Task<object> LoginPostRequest(AdminLogin dto)
        {
            HttpResponseMessage response = await _adminAuthClient.PostAsJsonAsync("api/adminauth/signin", dto);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<AdminLoginResponseDto>(responseBody, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                })!;
            }
            else
            {
                return await response.Content.ReadAsStringAsync();
            }
        }

        public async Task<string> SignupPostRequest(AdminDto dto)
        {
            var response = await _adminAuthClient.PostAsJsonAsync("api/adminauth/signup", dto);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
