namespace CRM_API.Models.DTOs;

public class ClientDTO
{
    public int Id { get; set; }
    public string ProfilePictureUrl {  get; set; }
    public string Title { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string ContactNumber { get; set; } = string.Empty;
    public string AddressInformation { get; set; } = string.Empty;
    public string ClientType { get; set; } = string.Empty;

    public ClientDTO(Client client)
    {
        Id = client.Id;
        ProfilePictureUrl = $"/client/{client.Id}/profile-picture";
        Title = client.Title;
        Name = client.Name;
        Surname = client.Surname;
        Email = client.Email;
        ContactNumber = client.ContactNumber ?? string.Empty;
        AddressInformation = client.AddressInformation ?? string.Empty;
        ClientType = client.ClientType;
    }
}
