namespace EmployeeWebApp.Models.DTOs;

public class EmployeeDTO
{

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Surname { get; set; } = string.Empty;

    [Required]
    public string Email { get; set; } = string.Empty;

}
