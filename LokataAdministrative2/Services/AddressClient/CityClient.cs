using LokataAdministrative2.Models;
using System.Net.Http;
using System.Net.Http.Json;

namespace LokataAdministrative2.Services
{
    public interface ICityClient : IClient<CityDto> 
    {
        Task<List<CityDto>> GetRequestByProvinceId(string id);
    }

    public class CityClient : ICityClient
    {
        private readonly HttpClient cityClient;

        public CityClient(HttpClient cityClient)
        {
            this.cityClient = cityClient;
        }

        public Task DeleteRequest(string id)
        {
            throw new NotImplementedException();
        }

        public Task<List<CityDto>?> GetAllRequest()
        {
            throw new NotImplementedException();
        }

        public Task<CityDto?> GetRequestById(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<CityDto>> GetRequestByProvinceId(string id)
        {
            var response = await cityClient.GetAsync($"api/address/city/{id}");
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<List<CityDto>>();
        }

        public Task PostRequest(CityDto dto)
        {
            throw new NotImplementedException();
        }

        public Task PutRequest(CityDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
