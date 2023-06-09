namespace LokataAdministrative2.Services
{
    public interface IClient<T>
    {
        Task PostRequest(T dto, string token);
        Task PutRequest(T dto, string token); 
        Task DeleteRequest(string id, string token);
        Task<T?> GetRequestById(string id, string token);
        Task<List<T>> GetAllRequest(string token);
        void AuthenticateToken(string token);
    }
}
