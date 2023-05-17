using LokataAdministrative2.Models;
using LokataAdministrative2.Models.Citation;
using System.Net.Http.Json;

namespace LokataAdministrative2.Services
{
    public interface IViolationFeeClient : IClient<ViolationFeeDto> 
    { 
        Task<List<ViolationFeeDto>> GetRequestByViolationId(string id); 
    }
    public class ViolationFeeClient : IViolationFeeClient
    {
        private readonly HttpClient violationFeeClient;

        public ViolationFeeClient(HttpClient violationFeeClient)
        {
            this.violationFeeClient = violationFeeClient;
        }

        public Task PostRequest(ViolationFeeDto dto)
        {
            throw new NotImplementedException();
        }

        public Task PutRequest(ViolationFeeDto dto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteRequest(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ViolationFeeDto?> GetRequestById(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ViolationFeeDto>> GetRequestByViolationId(string id)
        {
            var response = await violationFeeClient.GetAsync($"api/violations/fees/{id}");
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<List<ViolationFeeDto>>();
        }

        public Task<List<ViolationFeeDto>?> GetAllRequest()
        {
            throw new NotImplementedException();
        }
    }
}
