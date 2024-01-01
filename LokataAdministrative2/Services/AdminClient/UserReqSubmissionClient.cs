using LokataAdministrative2.Models.Users;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace LokataAdministrative2.Services.AdminClient
{
    public interface IUserReqSubmissionClient : IClient<UserRequirement> 
    {
        Task<UserRequirement> GetByTctNo(string tctNo, string token);
    }

    public class UserReqSubmissionClient : IUserReqSubmissionClient
    {
        private readonly HttpClient userRequirements;

        public UserReqSubmissionClient(HttpClient userReq)
        {
            userRequirements = userReq;
        }

        public void AuthenticateToken(string token)
        {
            userRequirements.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task PostRequest(UserRequirement dto, string token)
        {
            AuthenticateToken(token);
            await userRequirements.PostAsJsonAsync("api/usersubmission/requirements", dto);
        }

        public async Task PutRequest(UserRequirement dto, string token)
        {
            AuthenticateToken(token);
            await userRequirements.PutAsJsonAsync($"api/usersubmission/requirements/{dto.Id}", dto);
        }

        public async Task DeleteRequest(string id, string token)
        {
            AuthenticateToken(token);
            await userRequirements.DeleteAsync($"api/usersubmission/requirements{id}");
        }

        public async Task<UserRequirement?> GetRequestById(string id, string token)
        {
            AuthenticateToken(token);
            var response = await userRequirements.GetAsync($"api/usersubmission/requirements/{id}");
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<UserRequirement>();
        }

        public async Task<List<UserRequirement>> GetAllRequest(string token)
        {
            AuthenticateToken(token);
            var response = await userRequirements.GetAsync($"api/usersubmission/requirements");
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<List<UserRequirement>>();
        }

        public async Task<UserRequirement> GetByTctNo(string tctNo, string token)
        {
            AuthenticateToken(token);
            var response = await userRequirements.GetAsync($"api/usersubmission/requirements/tctno/{tctNo}");
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<UserRequirement>();
        }
    }
}
