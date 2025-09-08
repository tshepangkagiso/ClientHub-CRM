namespace EmployeeWebApp.Models;

public class LoginDetails
{
	public int Id { get; set; }
    [Required]
    public int UserId { get; set; }

    [Required]
    public string UserType { get; set; } = string.Empty;

    [Required]
    public string Username { get; set; } = string.Empty;

	[Required]
	public string Password { get; set; } = string.Empty;

    public LoginDetails(int userId,string userType,string username, string password)
    {
        UserId = userId;
        UserType = userType;
        Username = username;
        Password = password;
    }

    public LoginDetails(int id, int userId, string userType,string username, string password)
    {
        Id = id;
        UserId = userId;
        UserType = userType;
        Username = username;
        Password = password;
    }
}
