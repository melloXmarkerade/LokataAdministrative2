using LokataAdministrative2.Models;
using LokataAdministrative2.Models.Users;
using System.Net.Http.Json;

namespace LokataAdministrative2.Services
{
    public interface IPaymentChannelClient : IClient<PaymentChannelDto> { }

    public class PaymentChannelClient : IPaymentChannelClient
    {
        private readonly HttpClient paymentClient;

        public PaymentChannelClient(HttpClient paymentClient)
        {
            this.paymentClient = paymentClient;
        }

        public void AuthenticateToken(string token)
        {
            throw new NotImplementedException();
        }

        public async Task PostRequest(PaymentChannelDto dto, string token)
        {
            await paymentClient.PostAsJsonAsync("api/paymentchannel", dto);
        }

        public async Task PutRequest(PaymentChannelDto dto, string token)
        {
            await paymentClient.PutAsJsonAsync($"api/paymentchannel/{dto.Id}", dto);
        }

        public async Task DeleteRequest(string id, string token)
        {
            await paymentClient.DeleteAsync($"api/paymentchannel/{id}");
        }

        public async Task<PaymentChannelDto?> GetRequestById(string id, string token)
        {
            var response = await paymentClient.GetAsync($"api/paymentchannel/{id}");
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<PaymentChannelDto>();
        }

        public async Task<List<PaymentChannelDto>> GetAllRequest(string token)
        {
            var response = await paymentClient.GetAsync($"api/paymentchannel");
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<List<PaymentChannelDto>>();
        }
    }
}