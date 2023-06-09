using LokataAdministrative2.Models;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace LokataAdministrative2.Services
{
    public interface IProvinceClient : IClient<ProvinceDto> { }

    public class ProvinceClient : IProvinceClient
    {
        private readonly HttpClient provinceClient;

        public ProvinceClient(HttpClient provinceClient) => this.provinceClient = provinceClient;

        public Task PostRequest(ProvinceDto dto, string token)
        {
            AuthenticateToken(token);
            throw new NotImplementedException();
        }

        public Task PutRequest(ProvinceDto dto, string token)
        {
            AuthenticateToken(token);
            throw new NotImplementedException();
        }


        public Task DeleteRequest(string id, string token)
        {
            AuthenticateToken(token);
            throw new NotImplementedException();
        }

        public Task<ProvinceDto?> GetRequestById(string id, string token)
        {
            AuthenticateToken(token);
            throw new NotImplementedException();
        }

        public async Task<List<ProvinceDto>?> GetAllRequest(string token)
        {
            AuthenticateToken(token);
            var response = await provinceClient.GetAsync($"/api/address/province");
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<List<ProvinceDto>>();
        }

        public void AuthenticateToken(string token)
        {
            provinceClient.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
