namespace CRM_API.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthorityController : ControllerBase
{
    private readonly IConfiguration configuration;
    private readonly IAuthenticator authenticator;
    public AuthorityController(IConfiguration configuration, IAuthenticator authenticator)
    {
        this.configuration = configuration;
        this.authenticator = authenticator;
    }


    [HttpPost("auth")]
    public async Task<IActionResult> Authenticate([FromBody] LoginDTO credential)
    {
        var expiresAt = DateTime.UtcNow.AddMinutes(10);
        var secretkey = configuration.GetValue<string>("SecretKey");

        var (token, userId) = await authenticator.Authenticate(credential.Username, credential.Password, expiresAt, secretkey);

        if (token != null)
        {
            return Ok(new
            {
                access_token = token,
                expires_at = expiresAt,
                user_id = userId
            });
        }
        else
        {
            ModelState.AddModelError("Unauthorized", "You are not authorized");
            var problemDetails = new ValidationProblemDetails(ModelState)
            {
                Status = StatusCodes.Status401Unauthorized
            };
            return new UnauthorizedObjectResult(problemDetails);
        }
    }


}
