using LokataAdministrative2.Models;
using System.Net.Http.Json;

namespace LokataAdministrative2.Services
{
    public interface ICitationClient : IClient<CitationDto> { }

    public class CitationClient : ICitationClient
    {
        private readonly HttpClient citationClient;

        public CitationClient(HttpClient httpClient)
        {
            citationClient = httpClient;
        }

        public async Task PostRequest(CitationDto dto)
        {
            await citationClient.PostAsJsonAsync("api/Citation", dto);
        }

        public async Task PutRequest(CitationDto dto)
        {
            await citationClient.PutAsJsonAsync($"api/Citation/{dto.Id}", dto);
        }

        public async Task DeleteRequest(string id)
        {
            await citationClient.DeleteAsync($"api/Citation/{id}");
        }

        public async Task<CitationDto?> GetRequestById(string id)
        {
            var response = await citationClient.GetAsync($"api/Citation/{id}");
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<CitationDto>();
        }

        public async Task<List<CitationDto>?> GetAllRequest()
        {
            var response = await citationClient.GetAsync($"/api/Citation");
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<List<CitationDto>>();
        }
    }
}
