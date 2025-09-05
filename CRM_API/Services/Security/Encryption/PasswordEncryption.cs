namespace CRM_API.Services.Security.Encryption;
using BCrypt.Net;

public class PasswordEncryption : IPasswordEncryption
{
    private const int SALT_ROUNDS = 12;

    public string HashPassword(string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            throw new ArgumentException("Password cannot be null or empty", nameof(password));
        }

        try
        {
            return BCrypt.HashPassword(password, SALT_ROUNDS);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Password Error: Failed to hash password. {ex.Message}");
            throw new InvalidOperationException("Failed to hash password", ex);
        }
    }

    public bool VerifyPassword(string password,string hashedPassword)
    {
        try
        {
            bool isVerified = BCrypt.Verify(password, hashedPassword);
            return isVerified;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Password Error: Failed to encrpyt");
            return false;
        }
    }
}
