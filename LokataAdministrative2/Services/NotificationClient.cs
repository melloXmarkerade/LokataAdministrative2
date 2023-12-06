using LokataAdministrative2.Models;
using System.Net.Http.Json;

namespace LokataAdministrative2.Services
{
    public interface INotificationClient : IClient<NotificationDto> { }

    public class NotificationClient : INotificationClient
    {
        private HttpClient notificationClient;

        public NotificationClient(HttpClient notificationClient)
        {
            this.notificationClient = notificationClient;
        }

        public void AuthenticateToken(string token)
        {
            throw new NotImplementedException();
        }

        public Task DeleteRequest(string id, string token)
        {
            throw new NotImplementedException();
        }

        public Task<List<NotificationDto>> GetAllRequest(string token)
        {
            throw new NotImplementedException();
        }

        public Task<NotificationDto?> GetRequestById(string id, string token)
        {
            throw new NotImplementedException();
        }

        public async Task PostRequest(NotificationDto dto, string token)
        {
            await notificationClient.PostAsJsonAsync("api/notifications", dto);
        }

        public Task PutRequest(NotificationDto dto, string token)
        {
            throw new NotImplementedException();
        }
    }
}
