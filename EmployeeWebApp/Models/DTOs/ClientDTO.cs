namespace EmployeeWebApp.Models.DTOs;

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

    public ClientDTO() { }
}
