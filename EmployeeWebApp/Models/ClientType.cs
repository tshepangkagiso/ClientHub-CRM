namespace EmployeeWebApp.Models;

public class ClientType
{
    public int Id { get; set; }
    public string Type { get; set; } = string.Empty;

    public ClientType(string type)
    {
        Type = type;   
    }
}