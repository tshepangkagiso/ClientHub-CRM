namespace CRM_API.Models.DTOs;

public class UpdateClientDTO
{
    public int Id { get; set; }
    public IFormFile? ProfilePicture { get; set; }
    public string? Title { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? Email { get; set; }
    public string? ContactNumber { get; set; }
    public string? AddressInformation { get; set; }
    public string? ClientType { get; set; }
}
