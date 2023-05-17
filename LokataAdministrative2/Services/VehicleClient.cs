using LokataAdministrative2.Models;
using System.Net.Http.Json;

namespace LokataAdministrative2.Services
{
    public interface IVehicleClient : IClient<VehicleDto> { }

    public class VehicleClient : IVehicleClient
    {
        private HttpClient vehicleImpoundedClient;

        public VehicleClient(HttpClient vehicleImpoundedClient)
        {
            this.vehicleImpoundedClient = vehicleImpoundedClient;
        }

        public async Task PostRequest(VehicleDto dto)
        {
            await vehicleImpoundedClient.PostAsJsonAsync("api/Vehicle", dto);
        }

        public async Task PutRequest(VehicleDto dto)
        {
            await vehicleImpoundedClient.PutAsJsonAsync($"api/Vehicle/{dto.Id}", dto);
        }

        public async Task DeleteRequest(string id)
        {
            await vehicleImpoundedClient.DeleteAsync($"api/Vehicle/{id}");
        }

        public async Task<VehicleDto?> GetRequestById(string id)
        {
            var response = await vehicleImpoundedClient.GetAsync($"api/Vehicle/{id}");
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<VehicleDto>();
        }

        public async Task<List<VehicleDto>?> GetAllRequest()
        {
            var response = await vehicleImpoundedClient.GetAsync($"api/Vehicle");
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<List<VehicleDto>>();
        }
    }
}
