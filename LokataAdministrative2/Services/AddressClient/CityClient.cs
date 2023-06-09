using LokataAdministrative2.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace LokataAdministrative2.Services
{
    public interface ICityClient : IClient<CityDto> 
    {
        Task<List<CityDto>?> GetRequestByProvinceId(string id, string token);
    }

    public class CityClient : ICityClient
    {
        private readonly HttpClient cityClient;

        public CityClient(HttpClient cityClient)
        {
            this.cityClient = cityClient;
        }

        public void AuthenticateToken(string token)
        {
            cityClient.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Bearer", token);
        }

        public Task DeleteRequest(string id, string token)
        {
            throw new NotImplementedException();
        }

        public async Task<List<CityDto>> GetAllRequest(string token)
        {
            AuthenticateToken(token);
            var response = await cityClient.GetAsync($"api/address/city");
            if (!response.IsSuccessStatusCode)
                return null;

            return response.Content.ReadFromJsonAsync<List<CityDto>>().Result!;
        }

        public Task<CityDto?> GetRequestById(string id, string token)
        {
            throw new NotImplementedException();
        }

        public async Task<List<CityDto>?> GetRequestByProvinceId(string id, string token)
        {
            AuthenticateToken(token);
            var response = await cityClient.GetAsync($"api/address/city/{id}");
            if (!response.IsSuccessStatusCode)
                return null;

            return response.Content.ReadFromJsonAsync<List<CityDto>>().Result!;
        }

        public Task PostRequest(CityDto dto, string token)
        {
            throw new NotImplementedException();
        }

        public Task PutRequest(CityDto dto, string token)
        {
            throw new NotImplementedException();
        }
    }
}
