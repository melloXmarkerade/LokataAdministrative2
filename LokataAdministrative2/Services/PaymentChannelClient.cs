using LokataAdministrative2.Models;

namespace LokataAdministrative2.Services
{
    public interface IPaymentChannel : IClient<PaymentChannelDto> { }

    public class PaymentChannelClient : IPaymentChannel
    {
        public void AuthenticateToken(string token)
        {
            throw new NotImplementedException();
        }

        public Task PostRequest(PaymentChannelDto dto, string token)
        {
            throw new NotImplementedException();
        }

        public Task PutRequest(PaymentChannelDto dto, string token)
        {
            throw new NotImplementedException();
        }

        public Task DeleteRequest(string id, string token)
        {
            throw new NotImplementedException();
        }

        public Task<PaymentChannelDto?> GetRequestById(string id, string token)
        {
            throw new NotImplementedException();
        }

        public Task<List<PaymentChannelDto>> GetAllRequest(string token)
        {
            throw new NotImplementedException();
        }
    }
}