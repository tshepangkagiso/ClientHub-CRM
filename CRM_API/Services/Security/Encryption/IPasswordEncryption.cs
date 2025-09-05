namespace CRM_API.Services.Security.Encryption;

public interface IPasswordEncryption
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hashedPassword);
}
