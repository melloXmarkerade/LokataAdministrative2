using LokataAdministrative2.Models;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace LokataAdministrative2.Services
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
    }
}
