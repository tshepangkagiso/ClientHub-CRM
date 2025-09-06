namespace CRM_API.Models;

public class Employee
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public string Surname { get; set; } = string.Empty;
    [Required]
    public string Email { get; set; } = string.Empty;

    public Employee()
    {
        
    }
    public Employee(int id , string name, string surname, string email)
    {
        this.Id = id;
        this.Name = name;
        this.Surname = surname;
        this.Email = email;
    }

    public Employee(string name, string surname, string email)
    {
        this.Name = name;
        this.Surname = surname;
        this.Email = email;
    }
}
