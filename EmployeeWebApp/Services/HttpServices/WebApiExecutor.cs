using System.Text.Json;

namespace EmployeeWebApp.Services.HttpServices;

public class WebApiExecutor : IWebApiExecutor
{
    private const string apiName = "CRM_API";
    private readonly IHttpClientFactory httpClientFactory;
    public WebApiExecutor(IHttpClientFactory httpClientFactory)
    {
        this.httpClientFactory = httpClientFactory;
    }

    //Get all clients
    public async Task<T?> GetAllClients<T>()
    {
        var httpClient = httpClientFactory.CreateClient(apiName);
        return await httpClient.GetFromJsonAsync<T>("client");
    }


    //Get all titles
    public async Task<T?> GetTitles<T>()
    {
        var httpClient = httpClientFactory.CreateClient(apiName);
        return await httpClient.GetFromJsonAsync<T>("titles");
    }

    //Get all types
    public async Task<T?> GetTypes<T>()
    {
        var httpClient = httpClientFactory.CreateClient(apiName);
        return await httpClient.GetFromJsonAsync<T>("types");
    }

    //Create Client   //client
    public async Task<T?> CreateClient<T>(T obj)
    {
        var httpClient = httpClientFactory.CreateClient(apiName);
        var response = await httpClient.PostAsJsonAsync("client", obj);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<T>();
    }
    

    //Update Client //client

    //Update Clients By ClientTypes  //client/updateAll

    //Delete Client //client/{id}
}
