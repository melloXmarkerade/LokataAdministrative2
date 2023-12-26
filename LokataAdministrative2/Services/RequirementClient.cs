using LokataAdministrative2.Models;
using LokataAdministrative2.Models.Users;
using System.Net.Http.Json;

namespace LokataAdministrative2.Services
{
    public interface IRequirementClient : IClient<Requirement> { }

    public class RequirementClient : IRequirementClient
    {
        private readonly HttpClient requirementClient;

        public RequirementClient(HttpClient requirement)
        {
            this.requirementClient = requirement;
        }

        public void AuthenticateToken(string token)
        {
            throw new NotImplementedException();
        }

        public async Task PostRequest(Requirement dto, string token)
        {
            await requirementClient.PostAsJsonAsync("api/requirement", dto);
        }

        public async Task PutRequest(Requirement dto, string token)
        {
            await requirementClient.PutAsJsonAsync($"api/requirement/{dto.Id}", dto);
        }

        public async Task DeleteRequest(string id, string token)
        {
            await requirementClient.DeleteAsync($"api/requirement/{id}");
        }

        public async Task<Requirement?> GetRequestById(string id, string token)
        {
            var response = await requirementClient.GetAsync($"api/requirement/{id}");
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<Requirement>();
        }

        public async Task<List<Requirement>> GetAllRequest(string token)
        {
            var response = await requirementClient.GetAsync($"api/requirement");
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<List<Requirement>>();
        }
    }
}
