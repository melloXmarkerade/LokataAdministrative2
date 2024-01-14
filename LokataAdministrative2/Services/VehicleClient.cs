using LokataAdministrative2.Models;
using System.Net.Http.Json;

namespace LokataAdministrative2.Services
{
    public interface IVehicleClient : IClient<VehicleDto> 
    {
        Task<List<VehicleDto>> GetAllVehicleInfo(string licenseNo);
        Task<bool> CheckTctNoHasImpoundVehicle(string tctNo);
    }

    public class VehicleClient : IVehicleClient
    {
        private HttpClient vehicleClient;

        public VehicleClient(HttpClient vehicleImpoundedClient)
        {
            this.vehicleClient = vehicleImpoundedClient;
        }

        public async Task PostRequest(VehicleDto dto, string token)
        {
            AuthenticateToken(token);
            await vehicleClient.PostAsJsonAsync("api/Vehicle", dto);
        }

        public async Task PutRequest(VehicleDto dto, string token)
        {
            AuthenticateToken(token);
            await vehicleClient.PutAsJsonAsync($"api/Vehicle/{dto.Id}", dto);
        }

        public async Task DeleteRequest(string id, string token)
        {
            AuthenticateToken(token);
            await vehicleClient.DeleteAsync($"api/Vehicle/{id}");
        }

        public async Task<VehicleDto?> GetRequestById(string id, string token)
        {
            AuthenticateToken(token);
            var response = await vehicleClient.GetAsync($"api/Vehicle/{id}");
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<VehicleDto>();
        }

        public async Task<List<VehicleDto>> GetAllRequest(string token)
        {
            AuthenticateToken(token);
            var response = await vehicleClient.GetAsync($"api/Vehicle");
            if (!response.IsSuccessStatusCode)
                return null!;

            return await response.Content.ReadFromJsonAsync<List<VehicleDto>>();
        }

        public void AuthenticateToken(string token)
        {
            vehicleClient.DefaultRequestHeaders.Authorization = new("Bearer", token);
        }

        public async Task<List<VehicleDto>> GetAllVehicleInfo(string licenseNo)
        {
            var response = await vehicleClient.GetAsync($"api/Vehicle/admin/licenseno/{licenseNo}");
            if (!response.IsSuccessStatusCode)
                return null!;

            return await response.Content.ReadFromJsonAsync<List<VehicleDto>>();
        }

        public async Task<bool> CheckTctNoHasImpoundVehicle(string tctNo)
        {
            var response = await vehicleClient.GetAsync($"api/Vehicle/tctno/{tctNo}");
            if (!response.IsSuccessStatusCode)
                return false;

            return await response.Content.ReadFromJsonAsync<bool>();
        }
    }
}
