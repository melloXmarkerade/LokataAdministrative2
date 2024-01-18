using LokataAdministrative2.Models;
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

        public Task PostRequest(ViolationDto dto, string token)
        {
            throw new NotImplementedException();
        }

        public Task PutRequest(ViolationDto dto, string token)
        {
            throw new NotImplementedException();
        }

        public Task DeleteRequest(string id, string token)
        {
            throw new NotImplementedException();
        }

        public Task<ViolationDto?> GetRequestById(string id, string token)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ViolationDto>?> GetRequestByCategoryId(string id, string token)
        {
            AuthenticateToken(token);
            var response = await violationClient.GetAsync($"api/violations/{id}");
            if (!response.IsSuccessStatusCode)
                return null;

            return response.Content.ReadFromJsonAsync<List<ViolationDto>>().Result!;
        }

        public Task<List<ViolationDto>?> GetAllRequest(string token)
        {
            throw new NotImplementedException();
        }

        public void AuthenticateToken(string token)
        {
            violationClient.DefaultRequestHeaders.Authorization = new("Bearer", token);
        }
    }
}
