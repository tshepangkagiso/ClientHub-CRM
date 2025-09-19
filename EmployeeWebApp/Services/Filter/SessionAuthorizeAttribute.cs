namespace EmployeeWebApp.Services.Filter;

public class SessionAuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var token = context.HttpContext.Session.GetString("access_token");
        var expiresAtStr = context.HttpContext.Session.GetString("expires_at");

        bool isExpired = true;

        if (!string.IsNullOrEmpty(expiresAtStr) && DateTime.TryParse(expiresAtStr, null, System.Globalization.DateTimeStyles.RoundtripKind, out var expiresAt))
        {
            isExpired = expiresAt < DateTime.UtcNow;
        }

        if (string.IsNullOrEmpty(token) || isExpired)
        {
            context.Result = new RedirectToActionResult("Login", "Account", null);
        }
    }

}

