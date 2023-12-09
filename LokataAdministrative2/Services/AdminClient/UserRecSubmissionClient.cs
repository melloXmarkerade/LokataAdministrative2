using LokataAdministrative2.Models.Users;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace LokataAdministrative2.Services.AdminClient
{
    public interface IUserRecSubmissionClient : IClient<UserReceipt> { }

    public class UserRecSubmissionClient : IUserRecSubmissionClient
    {
        private readonly HttpClient userReceipts;

        public UserRecSubmissionClient(HttpClient userReceipts)
        {
            this.userReceipts = userReceipts;
        }

        public void AuthenticateToken(string token)
        {
            userReceipts.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task PostRequest(UserReceipt dto, string token)
        {
            AuthenticateToken(token);
            await userReceipts.PostAsJsonAsync("api/usersubmission/receipts", dto);
        }

        public async Task PutRequest(UserReceipt dto, string token)
        {
            AuthenticateToken(token);
            await userReceipts.PutAsJsonAsync($"api/usersubmission/receipts/{dto.Id}", dto);
        }

        public async Task DeleteRequest(string id, string token)
        {
            AuthenticateToken(token);
            await userReceipts.DeleteAsync($"api/usersubmission/receipts/{id}");
        }

        public async Task<UserReceipt?> GetRequestById(string id, string token)
        {
            AuthenticateToken(token);
            var response = await userReceipts.GetAsync($"api/usersubmission/receipts/{id}");
            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadFromJsonAsync<UserReceipt>();
        }

        public async Task<List<UserReceipt>> GetAllRequest(string token)
        {
            AuthenticateToken(token);
            var response = await userReceipts.GetAsync($"api/usersubmission/receipts");
            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadFromJsonAsync<List<UserReceipt>>();
        }
    }
}
