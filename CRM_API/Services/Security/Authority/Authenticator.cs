namespace CRM_API.Services.Security.Authority;

public class Authenticator : IAuthenticator
{
    private readonly ApplicationDbContext dbContext;
    private readonly IPasswordEncryption encryption;
    public Authenticator(ApplicationDbContext dbContext, IPasswordEncryption encryption)
    {
        this.dbContext = dbContext;
        this.encryption = encryption;
    }
    public async Task<(string?, int)> Authenticate(string username, string password, DateTime expiresAt, string secretKey)
    {
        var user = await dbContext.LoginDetails.FirstOrDefaultAsync(x => x.Username == username);

        if (user == null) return (null, 0); // username not found
        bool isVerified = encryption.VerifyPassword(password, user.Password);
        if (!isVerified) return (null, 0); // password mismatch

        switch (user.UserType)
        {
            case "Employee":
                var employee = await dbContext.Employee.FirstOrDefaultAsync(x => x.Id == user.UserId);
                if (employee == null) return (null, 0);
                return (CreateToken(username, expiresAt, secretKey, employee), employee.Id);

            case "Client":
                var client = await dbContext.Client.FirstOrDefaultAsync(x => x.Id == user.UserId);
                if (client == null) return (null, 0);
                return (CreateToken(username, expiresAt, secretKey, client), client.Id);

            default:
                return (null, 0);
        }
    }

    public string CreateToken(string username, DateTime expiresAt, string secretKey, Employee employee)
    {
        var secretkey = Encoding.ASCII.GetBytes(secretKey);

        var claims = new List<Claim>
            {
                new Claim("Email", employee.Email),
                new Claim("Role", "Employee"),
                new Claim("Read","true"),
                new Claim("Write", "true")
            };

        var jwt = new JwtSecurityToken(signingCredentials:
            new SigningCredentials(new SymmetricSecurityKey(secretkey), SecurityAlgorithms.HmacSha256Signature),
            claims: claims,
            expires: expiresAt,
            notBefore: DateTime.UtcNow
        );
        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    public string CreateToken(string username, DateTime expiresAt, string secretKey, Client client)
    {
        var secretkey = Encoding.ASCII.GetBytes(secretKey);

        var claims = new List<Claim>
        {
            new Claim("Email", client.Email),
            new Claim("Role", "Client"),
            new Claim("Read","true"),
            new Claim("Write", "true")
        };

        var jwt = new JwtSecurityToken(signingCredentials:
            new SigningCredentials(new SymmetricSecurityKey(secretkey), SecurityAlgorithms.HmacSha256Signature),
            claims: claims,
            expires: expiresAt,
            notBefore: DateTime.UtcNow
        );
        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    public static bool VerifyToken(string token, string secretKey)
    {
        if (string.IsNullOrEmpty(token)) return false;

        var SecretKey = Encoding.ASCII.GetBytes(secretKey);

        SecurityToken securityToken;
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(SecretKey),
                ValidateLifetime = true,
                ValidateAudience = false,
                ValidateIssuer = false,
                ClockSkew = TimeSpan.Zero
            }, out securityToken);
        }
        catch (SecurityTokenException)
        {
            return false;
        }
        catch
        {
            throw;
        }
        return securityToken != null;
    }
}
