using LokataAdministrative2.Models;
using LokataAdministrative2.Models.Users;
using LokataAdministrative2.Services.AdminClient;
using System.Net.Http.Json;

namespace LokataAdministrative2.Services.ViolationsClient
{
    public interface IViolationClient : IClient<ViolationDto>
    {
        Task<List<ViolationDto>> GetRequestByCategoryId(string id, string token);
    }
    public class ViolationClient : IViolationClient
    {
        private readonly HttpClient violationClient;

        public ViolationClient(HttpClient violationCatClient)
        {
            violationClient = violationCatClient;
        }

        public async Task PostRequest(ViolationDto dto, string token)
        {
            AuthenticateToken(token);
            await violationClient.PostAsJsonAsync("api/violations", dto);
        }

        public async Task PutRequest(ViolationDto dto, string token)
        {
            AuthenticateToken(token);
            await violationClient.PutAsJsonAsync($"api/violations/{dto.Id}", dto);
        }

        public async Task DeleteRequest(string id, string token)
        {
            AuthenticateToken(token);
            await violationClient.DeleteAsync($"api/violations/{id}");
        }

        public Task<ViolationDto?> GetRequestById(string id, string token)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ViolationDto>> GetRequestByCategoryId(string id, string token)
        {
            AuthenticateToken(token);
            var response = await violationClient.GetAsync($"api/violations/{id}");
            if (!response.IsSuccessStatusCode)
                return null!;

            return response.Content.ReadFromJsonAsync<List<ViolationDto>>().Result!;
        }

        public async Task<List<ViolationDto>> GetAllRequest(string token)
        {
            AuthenticateToken(token);
            var response = await violationClient.GetAsync($"api/violations");
            if (!response.IsSuccessStatusCode)
                return null!;

            return await response.Content.ReadFromJsonAsync<List<ViolationDto>>();
        }

        public void AuthenticateToken(string token)
        {
            violationClient.DefaultRequestHeaders.Authorization = new("Bearer", token);
        }
    }
}
