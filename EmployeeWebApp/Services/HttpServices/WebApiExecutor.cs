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

    //Get client by Id / client/{id}
    public async Task<T?> GetClientById<T>(int id)
    {
        var httpClient = httpClientFactory.CreateClient(apiName);
        return await httpClient.GetFromJsonAsync<T>($"client/{id}");
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
    public async Task<T?> CreateClient<T>(CreateClientDTO dto)
    {
        var httpClient = httpClientFactory.CreateClient(apiName);

        using var content = new MultipartFormDataContent();

        if (dto.ProfilePicture != null)
        {
            var fileStream = dto.ProfilePicture.OpenReadStream();
            var streamContent = new StreamContent(fileStream);
            streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(dto.ProfilePicture.ContentType);
            content.Add(streamContent, "ProfilePicture", dto.ProfilePicture.FileName);
        }

        content.Add(new StringContent(dto.Title), "Title");
        content.Add(new StringContent(dto.Name), "Name");
        content.Add(new StringContent(dto.Surname), "Surname");
        content.Add(new StringContent(dto.Email), "Email");
        content.Add(new StringContent(dto.ContactNumber), "ContactNumber");
        content.Add(new StringContent(dto.AddressInformation), "AddressInformation");
        content.Add(new StringContent(dto.ClientType), "ClientType");
        content.Add(new StringContent(dto.Username), "Username");
        content.Add(new StringContent(dto.Password), "Password");

        var response = await httpClient.PostAsync("client", content);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<T>();
    }

    //Update Client //client
    public async Task<T?> UpdateClient<T>(UpdateClientDTO dto)
    {
        var httpClient = httpClientFactory.CreateClient(apiName);
        using var content = new MultipartFormDataContent();

        content.Add(new StringContent(dto.Id.ToString()), "Id");

        if (dto.ProfilePicture != null)
        {
            var fileStream = dto.ProfilePicture.OpenReadStream();
            var streamContent = new StreamContent(fileStream);
            streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(dto.ProfilePicture.ContentType);
            content.Add(streamContent, "ProfilePicture", dto.ProfilePicture.FileName);
        }

        content.Add(new StringContent(dto.Title), "Title");
        content.Add(new StringContent(dto.Name), "Name");
        content.Add(new StringContent(dto.Surname), "Surname");
        content.Add(new StringContent(dto.Email), "Email");
        content.Add(new StringContent(dto.ContactNumber), "ContactNumber");
        content.Add(new StringContent(dto.AddressInformation), "AddressInformation");
        content.Add(new StringContent(dto.ClientType), "ClientType");

        var response = await httpClient.PutAsync("client", content);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<T>();
    }

    //Update Clients By ClientTypes  //client/updateAll
    public async Task UpdateClientType<T>(UpdateByClientType dto)
    {
        var httpClient = httpClientFactory.CreateClient(apiName);
        var response = await httpClient.PutAsJsonAsync("client/updateAll", dto);
        response.EnsureSuccessStatusCode();
    }

    //Delete Client //client/{id}
    public async Task DeleteClient(int id)
    {
        var httpClient = httpClientFactory.CreateClient(apiName);
        var response = await httpClient.DeleteAsync($"client/{id}");
        response.EnsureSuccessStatusCode();
    }
}
