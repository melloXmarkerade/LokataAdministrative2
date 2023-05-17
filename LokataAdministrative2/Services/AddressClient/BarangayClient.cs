using LokataAdministrative2.Models;
using System.Net.Http.Json;

namespace LokataAdministrative2.Services
{
    public interface IBarangayClient : IClient<BarangayDto>
    {
        Task<List<BarangayDto>> GetRequestByCityId(string id);
    }

    public class BarangayClient : IBarangayClient
    {
        private readonly HttpClient barangayClient;

        public BarangayClient(HttpClient barangayClient)
        {
            this.barangayClient = barangayClient;
        }

        public Task PostRequest(BarangayDto dto)
        {
            throw new NotImplementedException();
        }

        public Task PutRequest(BarangayDto dto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteRequest(string id)
        {
            throw new NotImplementedException();
        }

        public Task<BarangayDto?> GetRequestById(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<BarangayDto>> GetRequestByCityId(string cityId)
        {
            var response = await barangayClient.GetAsync($"api/address/barangay/{cityId}");
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<List<BarangayDto>>();
        }

        public Task<List<BarangayDto>?> GetAllRequest()
        {
            throw new NotImplementedException();
        }
    }
}
