using CRM_API.Services.Database;
using CRM_API.Services.Security.Encryption;
using Microsoft.AspNetCore.Mvc;

namespace CRM_API.Controllers;

[Route("[Controller]")]
[ApiController]
public class Login : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IPasswordEncryption passwordEncryption;
    public Login(ApplicationDbContext _context, IPasswordEncryption passwordEncryption)
    {
        this._context = _context;
        this.passwordEncryption = passwordEncryption;
    }
}
