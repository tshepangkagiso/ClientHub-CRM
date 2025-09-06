namespace CRM_API.Controllers;

[ApiController]
[Route("[controller]")] 
public class HealthController : ControllerBase
{
    // Inject your DbContext into the controller
    private readonly ApplicationDbContext _context;

    public HealthController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> CheckDatabaseConnection()
    {
        try
        {
            // This simple query just tests if it can connect and communicate with the DB
            var canConnect = await _context.Database.CanConnectAsync();

            if (canConnect)
            {
                return Ok(new { Message = "Database connection successful!" });
            }
            else
            {
                return StatusCode(500, new { Message = "Database connection failed." });
            }
        }
        catch (Exception ex)
        {
            // If an exception is thrown, the connection definitely failed
            return StatusCode(500, new { Message = "Database connection error.", Error = ex.Message });
        }
    }
}
