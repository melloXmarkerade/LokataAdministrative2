using LokataAdministrative2.Models;
using System.Net.Http.Json;

namespace LokataAdministrative2.Services
{
    public interface ICitationClient : IClient<CitationDTO> { }

    public class CitationClient : ICitationClient
    {
        private readonly HttpClient _httpClient;

        public CitationClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task DeleteRequest(string id)
        {
            await _httpClient.DeleteAsync($"api/Citation/{id}");
        }

        public async Task<List<CitationDTO>?> GetAllRequest()
        {
            var response = await _httpClient.GetAsync($"api/Citation");
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<List<CitationDTO>>();
        }

        public async Task<CitationDTO?> GetRequestById(string id)
        {
            var response = await _httpClient.GetAsync($"api/Citation/{id}");
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<CitationDTO>();
        }

        public async Task PostRequest(CitationDTO dto)
        {
            await _httpClient.PostAsJsonAsync("api/Citation", dto);
        }

        public async Task PutRequest(CitationDTO dto)
        {
            await _httpClient.PutAsJsonAsync($"api/Citation/{dto.Id}", dto);
        }
    }
}
