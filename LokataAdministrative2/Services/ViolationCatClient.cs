using LokataAdministrative2.Models;
using System.Net.Http.Json;

namespace LokataAdministrative2.Services
{
    public interface IViolationCategoryClient : IClient<ViolationCategoryDto> { }

    public class ViolationCatClient : IViolationCategoryClient
    {
        private readonly HttpClient violationCatClient;

        public ViolationCatClient(HttpClient violationCatClient)
        {
            this.violationCatClient = violationCatClient;
        }

        public Task PostRequest(ViolationCategoryDto dto)
        {
            throw new NotImplementedException();
        }

        public Task PutRequest(ViolationCategoryDto dto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteRequest(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ViolationCategoryDto?> GetRequestById(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ViolationCategoryDto>?> GetAllRequest()
        {
            var response = await violationCatClient.GetAsync($"/api/violations/category");
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<List<ViolationCategoryDto>>();
        }
    }
}
