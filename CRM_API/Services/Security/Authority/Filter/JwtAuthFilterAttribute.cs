namespace CRM_API.Services.Security.Authority.Filter;

public class JwtAuthFilterAttribute : Attribute, IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var bearerToken = authHeader.FirstOrDefault(); // get the first value
        if (string.IsNullOrEmpty(bearerToken) || !bearerToken.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        // Extract only the JWT string
        string rawToken = bearerToken.Substring("Bearer ".Length).Trim();

        // Get secret key
        var configuration = context.HttpContext.RequestServices.GetService<IConfiguration>();
        string secretkey = configuration.GetValue<string>("SecretKey");

        // Validate
        if (!Authenticator.VerifyToken(rawToken, secretkey))
        {
            context.Result = new UnauthorizedResult();
        }

    }
}
