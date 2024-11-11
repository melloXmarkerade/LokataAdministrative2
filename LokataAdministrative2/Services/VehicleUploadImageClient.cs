using LokataAdministrative2.Models.Citation;
using System.Net.Http.Json;

namespace LokataAdministrative2.Services
{
    public class VehicleUploadImageClient
    {
        private readonly HttpClient uploadVehicleClient;

        public VehicleUploadImageClient(HttpClient vehicleImageUploadClient)
        {
            this.uploadVehicleClient = vehicleImageUploadClient;
        }

        public async Task PostRequest(VehiclePictureUploadDto dto, string token)
        {
            AuthenticateToken(token);
            await uploadVehicleClient.PostAsJsonAsync("api/fileupload/vehiclepictures", dto);
        }

        private void AuthenticateToken(string token)
        {
            uploadVehicleClient.DefaultRequestHeaders.Authorization = new("Bearer", token);
        }
    }
}
