namespace CRM_API.Controllers;

[Route("[controller]")]
[ApiController]
public class TitlesController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    public TitlesController(ApplicationDbContext _context)
    {
        this._context = _context;   
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            var titles = await _context.Titles.Select(x => x.Title).ToListAsync();
            if(titles == null || titles.Count == 0)
            {
                return NotFound("Titles Not Found.");
            }
            else
            {
                return Ok(titles);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return BadRequest("Failed to get Titles.");
        }
    }
}
