namespace EmployeeWebApp.Services.HttpServices;
public interface IWebApiExecutor
{
    Task<T?> CreateClient<T>(CreateClientDTO dto);
    Task DeleteClient(int id);
    Task<T?> GetAllClients<T>();
    Task<T?> GetClientById<T>(int id);
    Task<T?> GetTitles<T>();
    Task<T?> GetTypes<T>();
    Task<bool> Login(UserLogin userLogin);
    Task<T?> UpdateClient<T>(UpdateClientDTO dto);
    Task UpdateClientType<T>(UpdateByClientType dto);
}
