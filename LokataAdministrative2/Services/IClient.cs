namespace LokataAdministrative2.Services
{
    public interface IClient<T>
    {
        Task PostRequest(T dto);
        Task PutRequest(T dto); 
        Task DeleteRequest(string id);
        Task<T?> GetRequestById(string id);
        Task<List<T>?> GetAllRequest();

    }
}
