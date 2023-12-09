using LokataAdministrative2.Models;
using System.Net.Http.Json;

namespace LokataAdministrative2.Services.ViolationsClient
{
    public interface IViolationCategoryClient : IClient<ViolationCategoryDto> { }

    public class ViolationCatClient : IViolationCategoryClient
    {
        private readonly HttpClient violationCatClient;

        public ViolationCatClient(HttpClient violationCatClient)
        {
            this.violationCatClient = violationCatClient;
        }

        public Task PostRequest(ViolationCategoryDto dto, string token)
        {
            throw new NotImplementedException();
        }

        public Task PutRequest(ViolationCategoryDto dto, string token)
        {
            throw new NotImplementedException();
        }

        public Task DeleteRequest(string id, string token)
        {
            throw new NotImplementedException();
        }

        public Task<ViolationCategoryDto?> GetRequestById(string id, string token)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ViolationCategoryDto>?> GetAllRequest(string token)
        {
            AuthenticateToken(token);
            var response = await violationCatClient.GetAsync($"/api/violations/category");
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<List<ViolationCategoryDto>>();
        }

        public void AuthenticateToken(string token)
        {
            violationCatClient.DefaultRequestHeaders.Authorization = new("Bearer", token);
        }
    }
}
