namespace CRM_API.Services.Security.Authority;

public static class Authenticator
{
    public static bool Authenticate(string username, string password)
    {
        var app = AppRepository.GetApplicationByUsername(username);
        if (app == null) return false;
        return (app.Username == username && app.Password == password);
    }

    public static string CreateToken(string username, DateTime expiresAt, string secretKey)
    {
        var secretkey = Encoding.ASCII.GetBytes(secretKey);
        var app = AppRepository.GetApplicationByUsername(username);

        var claims = new List<Claim>
            {
                new Claim("AppName", app?.ApplicationName??string.Empty),
                new Claim("Read", (app?.Scopes??string.Empty).Contains("read")?"true":"false"),
                new Claim("Write", (app?.Scopes??string.Empty).Contains("write")?"true":"false")
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
