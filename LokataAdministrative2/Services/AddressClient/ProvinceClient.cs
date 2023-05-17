using LokataAdministrative2.Models;
using System.Net.Http;
using System.Net.Http.Json;

namespace LokataAdministrative2.Services
{
    public interface IProvinceClient : IClient<ProvinceDto> { }

    public class ProvinceClient : IProvinceClient
    {
        private readonly HttpClient provinceClient;

        public ProvinceClient(HttpClient provinceClient)
        {
            this.provinceClient = provinceClient;
        }

        public Task PostRequest(ProvinceDto dto)
        {
            throw new NotImplementedException();
        }

        public Task PutRequest(ProvinceDto dto)
        {
            throw new NotImplementedException();
        }


        public Task DeleteRequest(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ProvinceDto?> GetRequestById(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ProvinceDto>?> GetAllRequest()
        {
            var response = await provinceClient.GetAsync($"/api/address/province");
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<List<ProvinceDto>>();
        }
    }
}
