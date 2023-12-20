using LokataAdministrative2.Models;
using System.Net.Http.Json;

namespace LokataAdministrative2.Services
{
    public class UserClient : IClient<UserDto>
    {
        private HttpClient userClient;

        public UserClient(HttpClient userClient)
        {
            this.userClient = userClient;
        }

        public void AuthenticateToken(string token)
        {
            throw new NotImplementedException();
        }

        public Task DeleteRequest(string id, string token)
        {
            throw new NotImplementedException();
        }

        public Task<List<UserDto>> GetAllRequest(string token)
        {
            throw new NotImplementedException();
        }

        public async Task<UserDto?> GetRequestById(string licenseId, string token)
        {
            var response = await userClient.GetAsync($"api/user/licenseno/{licenseId}");
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<UserDto>();
        }

        public Task PostRequest(UserDto dto, string token)
        {
            throw new NotImplementedException();
        }

        public Task PutRequest(UserDto dto, string token)
        {
            throw new NotImplementedException();
        }
    }
}
