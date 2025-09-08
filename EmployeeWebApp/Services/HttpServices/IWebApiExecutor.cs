

namespace EmployeeWebApp.Services.HttpServices;
public interface IWebApiExecutor
{
    Task<T?> CreateClient<T>(T obj);
    Task<T?> GetAllClients<T>();
    Task<T?> GetTitles<T>();
    Task<T?> GetTypes<T>();
}
