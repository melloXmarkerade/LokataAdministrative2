using LokataAdministrative2.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace LokataAdministrative2.Services
{
    public interface ICitationClient : IClient<CitationDto> 
    {
        Task<CitationDto> GetByTctNo(string tctNo, string token);
        Task<bool> CheckExistedTctNo(string tctNo, string token);
    }

    public class CitationClient : ICitationClient
    {
        private readonly HttpClient citationClient;

        public CitationClient(HttpClient httpClient) => citationClient = httpClient;

        public async Task PostRequest(CitationDto dto, string token)
        {
            AuthenticateToken(token);
            await citationClient.PostAsJsonAsync("api/Citation", dto);
        }

        public async Task PutRequest(CitationDto dto, string token)
        {
            AuthenticateToken(token);
            await citationClient.PutAsJsonAsync($"api/Citation/{dto.Id}", dto);
        }

        public async Task DeleteRequest(string id, string token)
        {
            AuthenticateToken(token);
            await citationClient.DeleteAsync($"api/Citation/{id}");
        }

        public async Task<CitationDto?> GetRequestById(string id, string token)
        {
            AuthenticateToken(token);
            var response = await citationClient.GetAsync($"api/Citation/{id}");
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<CitationDto>();
        }

        public async Task<bool> CheckExistedTctNo(string tctNo, string token)
        {
            AuthenticateToken(token);
            var response = await citationClient.GetAsync($"api/Citation/checktctno/{tctNo}");
            if (!response.IsSuccessStatusCode)
                return false;

            return await response.Content.ReadFromJsonAsync<bool>();
        }

        public async Task<List<CitationDto>> GetAllRequest(string token)
        {
            AuthenticateToken(token);
            var response = await citationClient.GetAsync($"/api/Citation");
            if (!response.IsSuccessStatusCode)
                return null;
                        
            return await response.Content.ReadFromJsonAsync<List<CitationDto>>();
        }

        public void AuthenticateToken(string token)
        {
            citationClient.DefaultRequestHeaders.Authorization 
                = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<CitationDto?> GetByTctNo(string tctNo, string token)
        {
            AuthenticateToken(token);
            var response = await citationClient.GetAsync($"api/Citation/tctno/{tctNo}");
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<CitationDto>();
        }
    }
}
