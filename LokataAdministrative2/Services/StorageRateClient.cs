using LokataAdministrative2.Models;
using System.Net.Http.Json;

namespace LokataAdministrative2.Services
{
    public interface IStorageRateClient : IClient<StorageRateDto> { }

    public class StorageRateClient : IStorageRateClient
    {
        private readonly HttpClient storageRateService;

        public StorageRateClient(HttpClient storageService) { storageRateService = storageService; }

        public void AuthenticateToken(string token)
        {
            storageRateService.DefaultRequestHeaders.Authorization = new("Bearer", token);
        }

        public async Task PostRequest(StorageRateDto dto, string token)
        {
            AuthenticateToken(token);
            await storageRateService.PostAsJsonAsync("api/storagerate", dto);
        }

        public async Task PutRequest(StorageRateDto dto, string token)
        {
            AuthenticateToken(token);
            await storageRateService.PutAsJsonAsync($"api/storagerate/{dto.Id}", dto);
        }

        public async Task DeleteRequest(string id, string token)
        {
            AuthenticateToken(token);
            await storageRateService.DeleteAsync($"api/storagerate/{id}");
        }

        public async Task<StorageRateDto?> GetRequestById(string id, string token)
        {
            AuthenticateToken(token);
            var response = await storageRateService.GetAsync($"api/storagerate/{id}");
            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadFromJsonAsync<StorageRateDto>();
        }

        public async Task<List<StorageRateDto>> GetAllRequest(string token)
        {
            AuthenticateToken(token);
            var response = await storageRateService.GetAsync("api/storagerate");
            if (!response.IsSuccessStatusCode) return null;

            return response.Content.ReadFromJsonAsync<List<StorageRateDto>>().Result!;
        }
    }
}
