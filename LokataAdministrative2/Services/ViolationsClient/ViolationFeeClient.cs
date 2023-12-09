using LokataAdministrative2.Models.Citation;
using System.Net.Http.Json;

namespace LokataAdministrative2.Services.ViolationsClient
{
    public interface IViolationFeeClient : IClient<ViolationFeeDto>
    {
        Task<List<ViolationFeeDto>?> GetRequestByViolationId(string id, string token);
    }
    public class ViolationFeeClient : IViolationFeeClient
    {
        private readonly HttpClient violationFeeClient;

        public ViolationFeeClient(HttpClient violationFeeClient)
        {
            this.violationFeeClient = violationFeeClient;
        }

        public Task PostRequest(ViolationFeeDto dto, string token)
        {
            throw new NotImplementedException();
        }

        public Task PutRequest(ViolationFeeDto dto, string token)
        {
            throw new NotImplementedException();
        }

        public Task DeleteRequest(string id, string token)
        {
            throw new NotImplementedException();
        }

        public Task<ViolationFeeDto?> GetRequestById(string id, string token)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ViolationFeeDto>?> GetRequestByViolationId(string id, string token)
        {
            AuthenticateToken(token);
            var response = await violationFeeClient.GetAsync($"api/violations/fees/{id}");
            if (!response.IsSuccessStatusCode)
                return null;

            return response.Content.ReadFromJsonAsync<List<ViolationFeeDto>>().Result!;
        }

        public Task<List<ViolationFeeDto>?> GetAllRequest(string token)
        {
            throw new NotImplementedException();
        }

        public void AuthenticateToken(string token)
        {
            violationFeeClient.DefaultRequestHeaders.Authorization = new("Bearer", token);
        }
    }
}
