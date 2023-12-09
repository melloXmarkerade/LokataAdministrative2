using LokataAdministrative2.Models;
using LokataAdministrative2.Services.ViolationsClient;
using System.Net.Http.Json;

namespace LokataAdministrative2.Services
{
    public interface ITowingRateClient : IClient<TowingRateDto> { }

    public class TowingRatesClient : ITowingRateClient
    {
        private readonly HttpClient towingRateService;

        public TowingRatesClient(HttpClient towingService) { towingRateService = towingService; }

        public void AuthenticateToken(string token)
        {
            towingRateService.DefaultRequestHeaders.Authorization = new("Bearer", token);
        }

        public async Task PostRequest(TowingRateDto dto, string token)
        {
            AuthenticateToken(token);
            await towingRateService.PostAsJsonAsync("api/towingrate", dto);
        }

        public async Task PutRequest(TowingRateDto dto, string token)
        {
            AuthenticateToken(token);
            await towingRateService.PutAsJsonAsync($"api/towingrate/{dto.Id}", dto);
        }

        public async Task DeleteRequest(string id, string token)
        {
            AuthenticateToken(token);
            await towingRateService.DeleteAsync($"api/towingrate/{id}");
        }

        public async Task<TowingRateDto?> GetRequestById(string id, string token)
        {
            AuthenticateToken(token);
            var response = await towingRateService.GetAsync($"api/towingrate/{id}");
            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadFromJsonAsync<TowingRateDto>();
        }

        public async Task<List<TowingRateDto>> GetAllRequest(string token)
        {
            AuthenticateToken(token);
            var response = await towingRateService.GetAsync("api/towingrate");
            if (!response.IsSuccessStatusCode) return null;

            return response.Content.ReadFromJsonAsync<List<TowingRateDto>>().Result!;
        }
    }
}
