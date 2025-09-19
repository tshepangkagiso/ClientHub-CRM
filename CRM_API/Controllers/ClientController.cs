namespace CRM_API.Controllers;

[Route("[controller]")]
[ApiController]

public class ClientController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IImageProcessor imageProcessor;
    private readonly IPasswordEncryption passwordEncryption;
    public ClientController(ApplicationDbContext _context, IImageProcessor imageProcessor, IPasswordEncryption passwordEncryption)
    {
        this._context = _context;
        this.imageProcessor = imageProcessor;
        this.passwordEncryption = passwordEncryption;
    }

    //Get Image
    [HttpGet("{id}/profile-picture")]
    public async Task<IActionResult> GetProfilePicture(int id)
    {
        try
        {
            byte[]? imageData = await _context.Client.Where(c => c.Id == id).Select(c => c.ProfilePicture).FirstOrDefaultAsync();
            if (imageData == null)
            {
                return NotFound("This client does not have a profile picture.");
            }

            if (imageData.Length == 0)
            {
                return NotFound("Profile picture data is empty.");
            }

            return File(imageData, "image/jpeg");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while retrieving the profile picture.");
        }
    }

    //Get all clients
    [HttpGet]
    [JwtAuthFilter]
    public async Task<IActionResult> OnGet()
    {
        try
        {
            var clients = await _context.Client.ToListAsync();
            var clientDtos = clients.Select(client => new ClientDTO(client)).ToList();
            return Ok(clientDtos);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.ToString()}");
            return BadRequest("Failed to get clients.");
        }
    }

    //Get client by id
    [HttpGet("{id}")]
    [JwtAuthFilter]
    public async Task<IActionResult> Get( [FromRoute] int id)
    {
        try
        {
            var client = await _context.Client.FindAsync(id);

            if (client == null)
                return NotFound();

            return Ok(new ClientDTO(client));
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error: {ex.ToString()}");
            return BadRequest("Failed to get client by id.");
        }
    }

    //Register a new client
    [HttpPost]
    [JwtAuthFilter]
    public async Task<IActionResult> Post([FromForm] CreateClientDTO createClientDTO) 
    {
        // Use a transaction to ensure all operations succeed or fail together
        await using var transaction = await _context.Database.BeginTransactionAsync(); 

        try
        {
            if (!ModelState.IsValid) return BadRequest("Invalid client data.");

            byte[] processedImage = imageProcessor.Process(createClientDTO.ProfilePicture);
            string passwordHash = passwordEncryption.HashPassword(createClientDTO.Password);

            // Validate title and clientType
            var isValidTitle = await _context.Titles.AnyAsync(t => t.Title == createClientDTO.Title);
            var isValidClientType = await _context.ClientType.AnyAsync(ct => ct.Type == createClientDTO.ClientType);

            if (!isValidTitle) return BadRequest("Invalid title.");
            if (!isValidClientType) return BadRequest("Invalid client type.");

            // Create and add client
            Client client = new Client(
                processedImage, createClientDTO.Title, createClientDTO.Name,
                createClientDTO.Surname, createClientDTO.Email, createClientDTO.ContactNumber,
                createClientDTO.AddressInformation, createClientDTO.ClientType
            );
            await _context.Client.AddAsync(client);
            await _context.SaveChangesAsync();

            var savedClient = await _context.Client.FirstOrDefaultAsync(c => c.Email == createClientDTO.Email);

            if (savedClient != null)
            {
                LoginDetails login = new LoginDetails(
                    userId: savedClient.Id,
                    userType: "Client",
                    username: createClientDTO.Username,
                    password: passwordHash
                );
                await _context.LoginDetails.AddAsync(login);

                await _context.SaveChangesAsync();

                // Finalize the transaction
                await transaction.CommitAsync(); 
                return Ok("Client registration successful.");
            }
            else
            {
                await transaction.RollbackAsync(); // roll back if client not found
                return NotFound("Client was not saved.");
            }

        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(); // Rollback on error
            Console.WriteLine($"Error: {ex.Message}");
            return StatusCode(500, "An internal error occurred. Please try again later.");
        }
    }

    //Update
    [HttpPut]
    [JwtAuthFilter]
    public async Task<IActionResult> Put([FromForm] UpdateClientDTO updateClientDTO)
    {
        try
        {
            var client = await _context.Client.FirstOrDefaultAsync(client => client.Id == updateClientDTO.Id);
            if (client != null)
            {

                if (updateClientDTO.ProfilePicture != null) client.ProfilePicture = imageProcessor.Process(updateClientDTO.ProfilePicture);

                if (!string.IsNullOrEmpty(updateClientDTO.Title))
                {
                    client.Title = updateClientDTO.Title;
                }

                if (!string.IsNullOrEmpty(updateClientDTO.Name))
                {
                    client.Name = updateClientDTO.Name;
                }

                if (!string.IsNullOrEmpty(updateClientDTO.Email))
                {
                    client.Email = updateClientDTO.Email; 
                }
                if (!string.IsNullOrEmpty(updateClientDTO.Surname))
                {
                    client.Surname = updateClientDTO.Surname;
                }
                if (!string.IsNullOrEmpty(updateClientDTO.AddressInformation))
                {
                    client.AddressInformation = updateClientDTO.AddressInformation;
                }
                if (!string.IsNullOrEmpty(updateClientDTO.ContactNumber))
                {
                    client.ContactNumber = updateClientDTO.ContactNumber;
                }
                if (!string.IsNullOrEmpty(updateClientDTO.ClientType))
                {
                    client.ClientType = updateClientDTO.ClientType;
                }

                _context.Client.Update(client);
                await _context.SaveChangesAsync();

                return Ok("Updated client succesfully.");
            }
            else
            {
                return NotFound("Client not found.");
            }

        }
        catch(Exception e)
        {
            Console.WriteLine("Error: " + e.Message);
            return BadRequest("Operation to update client has failed.");
        }
    }

    //Update all by ClientType
    [HttpPut("/Client/updateAll")]
    [JwtAuthFilter]
    public async Task<IActionResult> PutCategory(UpdateByClientType updateByClientType)
    {
        try
        {
            string? oldType = updateByClientType.OldType;
            string? newType = updateByClientType.NewType;

            var newTypeExists = await _context.ClientType.AnyAsync(ct => ct.Type == newType);

            if (!newTypeExists)
                return BadRequest($"Client type '{newType}' does not exist.");

            int rowsAffected = await _context.Database.ExecuteSqlRawAsync(
                "EXEC spUpdateClientsByType @OldType, @NewType",
                new SqlParameter("@OldType", oldType),
                new SqlParameter("@NewType", newType)
            );

            if (rowsAffected == 0)
            {
                return NotFound($"No clients found with type '{oldType}' or no changes were made.");
            }

            return Ok($"Successfully updated {rowsAffected} clients from '{oldType}' to '{newType}'");
        }
        catch (Exception e)
        {
            return BadRequest("An error occurred while updating clients.");
        }
    }

    //Delete
    [HttpDelete("{id}")]
    [JwtAuthFilter]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            int rowsAffected = await _context.Database
                .ExecuteSqlRawAsync("EXEC spDeleteClient @Id", new SqlParameter("@Id", id));

            if (rowsAffected == 0)
            {
                return NotFound($"Client was not found.");
            }

            return Ok($"Client  was successfully deleted.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            return StatusCode(500, "An error occurred while deleting the client.");
        }
    }



}
