namespace CRM_API.Controllers;

[Route("[controller]")]
[ApiController]
public class TypesController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    public TypesController(ApplicationDbContext context)
    {
        this._context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            var clientTypes = await _context.ClientType.Select(x => x.Type).ToListAsync();
            if(clientTypes ==  null || clientTypes.Count == 0)
            {
                return NotFound("Client Types Not Found.");
            }
            else
            {
                return Ok(clientTypes);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return BadRequest("Failed to get Client Types");
        }
    }
}
