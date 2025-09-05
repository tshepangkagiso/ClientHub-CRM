using System.ComponentModel.DataAnnotations;

namespace CRM_API.Models.DTOs;

public class EmployeeDTO
{
    public int Id { get; set; }
    public IFormFile ProfilePicture { get; set; }
    [Required]
    public string Title { get; set; } = string.Empty;
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public string Surname { get; set; } = string.Empty;
    [Required]
    public string Email { get; set; } = string.Empty;
    public string ContactNumber { get; set; } = string.Empty;
    public string AddressInformation { get; set; } = string.Empty;

    public EmployeeDTO(int id, IFormFile profilePicture, string title, string name, string surname, string email, string contactNumber, string addressInformation, string clientType)
    {
        this.Id = id;
        this.ProfilePicture = profilePicture;
        this.Title = title;
        this.Name = name;
        this.Surname = surname;
        this.Email = email;
        this.ContactNumber = contactNumber;
        this.AddressInformation = addressInformation;
    }

    public EmployeeDTO(IFormFile profilePicture, string title, string name, string surname, string email, string contactNumber, string addressInformation, string clientType)
    {
        this.ProfilePicture = profilePicture;
        this.Title = title;
        this.Name = name;
        this.Surname = surname;
        this.Email = email;
        this.ContactNumber = contactNumber;
        this.AddressInformation = addressInformation;
    }
}
