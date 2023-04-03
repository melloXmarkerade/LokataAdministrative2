namespace LokataAdministrative2.Services
{
    public interface IClient<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
    }
}
