using CRM_API.Services.Database;
using CRM_API.Services.ImageProcessing;
using CRM_API.Services.Security.Encryption;
using Microsoft.AspNetCore.Mvc;

namespace CRM_API.Controllers;

[Route("[Controller]")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IImageProcessor imageProcessor;
    private readonly IPasswordEncryption passwordEncryption;
    public EmployeeController(ApplicationDbContext _context, IImageProcessor imageProcessor, IPasswordEncryption passwordEncryption)
    {
        this._context = _context;
        this.imageProcessor = imageProcessor;
        this.passwordEncryption = passwordEncryption;
    }
}
