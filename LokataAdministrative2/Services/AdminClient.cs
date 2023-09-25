using LokataAdministrative2.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace LokataAdministrative2.Services
{
    public interface IAdminClient : IClient<AdminSignup> { }

    public class AdminClient : IAdminClient
    {
        private readonly HttpClient adminClient;

        public AdminClient(HttpClient adminClient) => this.adminClient = adminClient;

        public void AuthenticateToken(string token)
        {
            adminClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task PostRequest(AdminSignup dto, string token)
        {
            AuthenticateToken(token);
            await adminClient.PostAsJsonAsync("api/admin", dto);
        }

        public async Task PutRequest(AdminSignup dto, string token)
        {
            AuthenticateToken(token);
            await adminClient.PutAsJsonAsync($"api/admin/{dto.Id}", dto);
        }

        public async Task DeleteRequest(string id, string token)
        {
            AuthenticateToken(token);
            await adminClient.DeleteAsync($"api/admin/{id}");
        }

        public async Task<AdminSignup?> GetRequestById(string id, string token)
        {
            AuthenticateToken(token);
            var response = await adminClient.GetAsync($"api/admin/{id}");
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<AdminSignup>();
        }

        public async Task<List<AdminSignup>> GetAllRequest(string token)
        {
            AuthenticateToken(token);
            var response = await adminClient.GetAsync($"api/admin");
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<List<AdminSignup>>();
        }
    }
}