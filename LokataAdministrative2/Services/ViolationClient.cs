using LokataAdministrative2.Models;
using System.Net.Http.Json;

namespace LokataAdministrative2.Services
{
    public interface IViolationClient : IClient<ViolationDto>
    {
        Task<List<ViolationDto>> GetRequestByCategoryId(string id);
    }
    public class ViolationClient : IViolationClient
    {
        private readonly HttpClient violationClient;

        public ViolationClient(HttpClient violationCatClient)
        {
            this.violationClient = violationCatClient;
        }

        public Task PostRequest(ViolationDto dto)
        {
            throw new NotImplementedException();
        }

        public Task PutRequest(ViolationDto dto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteRequest(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ViolationDto?> GetRequestById(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ViolationDto>> GetRequestByCategoryId(string id)
        {
            var response = await violationClient.GetAsync($"api/violations/{id}");
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<List<ViolationDto>>();
        }

        public Task<List<ViolationDto>?> GetAllRequest()
        {
            throw new NotImplementedException();
        }
    }
}
