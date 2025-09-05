namespace CRM_API.Models;

public class ClientType
{
    public int Id { get; set; }
    public string Type { get; set; } = string.Empty;

    public ClientType(string type)
    {
        this.Type = type;   
    }
}