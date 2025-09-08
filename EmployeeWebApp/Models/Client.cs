namespace EmployeeWebApp.Models;

public class Client
{
	public int Id { get; set; }
	public byte[] ProfilePicture { get; set; }
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
    [Required]
    public string ClientType {  get; set; } = string.Empty;

    public Client(int id, byte[] profilePicture, string title, string name, string surname, string email, string contactNumber, string addressInformation, string clientType)
    {
        Id = id;
        ProfilePicture = profilePicture;
        Title = title;
        Name = name;
        Surname = surname;
        Email = email;
        ContactNumber = contactNumber;
        AddressInformation = addressInformation;
        ClientType = clientType;
    }

    public Client(byte[] profilePicture, string title, string name, string surname, string email, string contactNumber, string addressInformation, string clientType)
    {
        ProfilePicture = profilePicture;
        Title = title;
        Name = name;
        Surname = surname;
        Email = email;
        ContactNumber = contactNumber;
        AddressInformation = addressInformation;
        ClientType = clientType;
    }
}
