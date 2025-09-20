namespace CRM_API.Controllers;

[Route("[Controller]")]
[ApiController]

public class EmployeeController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IPasswordEncryption passwordEncryption;
    public EmployeeController(ApplicationDbContext _context, IPasswordEncryption passwordEncryption)
    {
        this._context = _context;
        this.passwordEncryption = passwordEncryption;
    }

    [HttpGet("{id}")]
    [JwtAuthFilter]
    public async Task<IActionResult> Get(int id)
    {
        try
        {
            var employee = await _context.Employee.FirstOrDefaultAsync(e => e.Id == id);
            if (employee == null) return NotFound("Employee with that Id was not found.");
            return Ok(employee);
        }
        catch(Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            return BadRequest("Failed to get employee");
        }
    }

    [HttpGet]
    [JwtAuthFilter]
    public async Task<IActionResult> Get()
    {
        try
        {
            var employees = await _context.Employee.ToListAsync();
            if (employees == null) return NotFound("Employees were not found.");
            return Ok(employees);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            return BadRequest("Failed to get employees");
        }
    }

    [HttpPost]
    [JwtAuthFilter]
    public async Task<IActionResult> Post(CreateEmployeeDTO createEmployeeDTO)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            if (!ModelState.IsValid) return BadRequest("Invalid employee data.");
            Employee employee = new Employee(createEmployeeDTO.Name, createEmployeeDTO.Surname,createEmployeeDTO.Email);
            await _context.Employee.AddAsync(employee);
            await _context.SaveChangesAsync();

            var savedEmployee = await _context.Employee.FirstOrDefaultAsync(e => e.Email ==  employee.Email);
            if(savedEmployee != null)
            {
                string hashedPassword = passwordEncryption.HashPassword(createEmployeeDTO.Password);
                LoginDetails loginDetails = new LoginDetails(savedEmployee.Id, "Employee", createEmployeeDTO.Username,hashedPassword);

                await _context.LoginDetails.AddAsync(loginDetails);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return Ok("Client registration successful.");
            }
            else
            {
                await transaction.RollbackAsync();
                return NotFound("Employee was not saved.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            return BadRequest("Failed to create employee");
        }
    }


    /*  {
          "name": "Admin",
          "surname": "User",
          "email": "admin@clienthub.com",
          "username": "Admin1",
          "password": "password"
        }
    */
}
