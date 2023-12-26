using LokataAdministrative2.Models;
using LokataAdministrative2.Models.Citation;
using System.Net.Http.Json;

namespace LokataAdministrative2.Services
{
    public interface IImpoundingAreaClient : IClient<ImpoundedArea> { }

    public class ImpoundingAreaClient : IImpoundingAreaClient
    {
        private readonly HttpClient impoundingService;

        public ImpoundingAreaClient(HttpClient impoundingService)
        {
            this.impoundingService = impoundingService;
        }

        public void AuthenticateToken(string token)
        {
            impoundingService.DefaultRequestHeaders.Authorization = new("Bearer", token);
        }

        public async Task PostRequest(ImpoundedArea dto, string token)
        {
            AuthenticateToken(token);
            await impoundingService.PostAsJsonAsync("api/tracking", dto);
        }

        public async Task PutRequest(ImpoundedArea dto, string token)
        {
            AuthenticateToken(token);
            await impoundingService.PutAsJsonAsync($"api/tracking/{dto.Id}", dto);
        }

        public async Task DeleteRequest(string id, string token)
        {
            AuthenticateToken(token);
            await impoundingService.DeleteAsync($"api/tracking/{id}");
        }

        public async Task<ImpoundedArea?> GetRequestById(string id, string token)
        {
            AuthenticateToken(token);
            var response = await impoundingService.GetAsync($"api/tracking/{id}");
            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadFromJsonAsync<ImpoundedArea>();
        }

        public async Task<List<ImpoundedArea>> GetAllRequest(string token)
        {
            AuthenticateToken(token);
            var response = await impoundingService.GetAsync("api/tracking");
            if (!response.IsSuccessStatusCode) return null;

            return response.Content.ReadFromJsonAsync<List<ImpoundedArea>>().Result!;
        }
    }
}
