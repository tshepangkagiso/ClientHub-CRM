namespace EmployeeWebApp.Models.DTOs;

public class UpdateByClientType
{
    [Required]
    public string? OldType { get; set; }

    [Required]
    public string? NewType { get; set; }
}
