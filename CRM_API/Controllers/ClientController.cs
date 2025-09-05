using CRM_API.Models;
using CRM_API.Models.DTOs;
using CRM_API.Services.Database;
using CRM_API.Services.ImageProcessing;
using CRM_API.Services.Security.Encryption;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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

    //Read
    [HttpGet]
    public async Task<IActionResult> OnGet()
    {
        try
        {
            var clients = await _context.Client.ToListAsync();
            return Ok(clients);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.ToString()}");
            return BadRequest("Failed to get clients.");
        }
    }

    //Read 
    [HttpGet("{id}")]
    public async Task<IActionResult> Get( [FromRoute] int id)
    {
        if(id == 0)
        {
            return BadRequest("Need Client Id To Get A Client.");
        }

        try
        {
            var client = await _context.Client.FirstOrDefaultAsync(client => client.Id == id);
            if (client == null) return NotFound($"Client With Id: {id} Was Not Found.");
            return Ok(client);
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error: {ex.ToString()}");
            return BadRequest("Failed to get client by id.");
        }
    }

    //Create 
    [HttpPost]
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
    //Update all by category
    //Delete
}
