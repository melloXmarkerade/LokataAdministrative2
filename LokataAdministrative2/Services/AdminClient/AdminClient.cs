﻿using LokataAdministrative2.Models.Users;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace LokataAdministrative2.Services.AdminClient
{
    public interface IAdminClient : IClient<AdminDto>
    {
        Task<List<AdminDto>> GetRegisteredAccounts(string token);
        Task<AdminDto> GetRequestByName(string name, string token);
    }

    public class AdminClient : IAdminClient
    {
        private readonly HttpClient adminClient;

        public AdminClient(HttpClient adminClient) => this.adminClient = adminClient;

        public void AuthenticateToken(string token)
        {
            adminClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task PostRequest(AdminDto dto, string token)
        {
            AuthenticateToken(token);
            await adminClient.PostAsJsonAsync("api/admin", dto);
        }

        public async Task PutRequest(AdminDto dto, string token)
        {
            AuthenticateToken(token);
            await adminClient.PutAsJsonAsync($"api/admin/{dto.Id}", dto);
        }

        public async Task DeleteRequest(string id, string token)
        {
            AuthenticateToken(token);
            await adminClient.DeleteAsync($"api/admin/{id}");
        }

        public async Task<AdminDto?> GetRequestById(string id, string token)
        {
            AuthenticateToken(token);
            var response = await adminClient.GetAsync($"api/admin/{id}");
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<AdminDto>();
        }

        public async Task<AdminDto?> GetRequestByName(string name, string token)
        {
            AuthenticateToken(token);
            var response = await adminClient.GetAsync($"api/admin/admins/{name}");
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<AdminDto>();
        }

        public async Task<List<AdminDto>> GetAllRequest(string token)
        {
            AuthenticateToken(token);
            var response = await adminClient.GetAsync($"api/admin/pendingaccounts");
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<List<AdminDto>>();
        }

        public async Task<List<AdminDto>> GetRegisteredAccounts(string token)
        {
            AuthenticateToken(token);
            var response = await adminClient.GetAsync($"api/admin/registeredaccounts");
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<List<AdminDto>>();
        }
    }
}