using LokataAdministrative2.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace LokataAdministrative2.Services
{
    public interface IBarangayClient : IClient<BarangayDto>
    {
        Task<List<BarangayDto>> GetRequestByCityId(string id, string token);
    }

    public class BarangayClient : IBarangayClient
    {
        private readonly HttpClient barangayClient;

        public BarangayClient(HttpClient barangayClient)
        {
            this.barangayClient = barangayClient;
        }

        public Task PostRequest(BarangayDto dto, string token)
        {
            throw new NotImplementedException();
        }

        public Task PutRequest(BarangayDto dto, string token)
        {
            throw new NotImplementedException();
        }

        public Task DeleteRequest(string id, string token)
        {
            throw new NotImplementedException();
        }

        public Task<BarangayDto?> GetRequestById(string id, string token)
        {
            throw new NotImplementedException();
        }

        public async Task<List<BarangayDto>?> GetRequestByCityId(string cityId, string token)
        {
            AuthenticateToken(token);
            var response = await barangayClient.GetAsync($"api/address/barangay/{cityId}");
            if (!response.IsSuccessStatusCode)
                return null;

            return response.Content.ReadFromJsonAsync<List<BarangayDto>>().Result!;
        }

        public Task<List<BarangayDto>?> GetAllRequest(string token)
        {
            throw new NotImplementedException();
        }

        public Task<List<BarangayDto>> GetRequestByCityId(string id)
        {
            throw new NotImplementedException();
        }

        public void AuthenticateToken(string token)
        {
            barangayClient.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
