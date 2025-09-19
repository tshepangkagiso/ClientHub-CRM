namespace CRM_API.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthorityController : ControllerBase
{
    private readonly IConfiguration configuration;
    public AuthorityController(IConfiguration configuration)
    {
        this.configuration = configuration;
    }


    [HttpPost("auth")]
    public IActionResult Authenticate([FromBody] AppCredential credential)
    {
        if(Authenticator.Authenticate(credential.Username, credential.Password))
        {
            var expiresAt = DateTime.UtcNow.AddMinutes(10);
            var secretkey = configuration.GetValue<string>("SecretKey");
            return Ok(new
            {
                access_token = Authenticator.CreateToken(credential.Username, expiresAt, secretkey),
                expires_at = expiresAt
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
