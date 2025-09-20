
namespace CRM_API.Services.Security.Authority
{
    public interface IAuthenticator
    {
        Task<(string?, int)> Authenticate(string username, string password, DateTime expiresAt, string secretKey);
        string CreateToken(string username, DateTime expiresAt, string secretKey, Client client);
        string CreateToken(string username, DateTime expiresAt, string secretKey, Employee employee);
        //bool VerifyToken(string token, string secretKey);
    }
}