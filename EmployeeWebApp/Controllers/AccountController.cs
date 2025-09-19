namespace EmployeeWebApp.Controllers;

[Route("[controller]")]
public class AccountController : Controller
{
    private readonly IWebApiExecutor webApiExecutor;

    public AccountController(IWebApiExecutor webApiExecutor)
    {
        this.webApiExecutor = webApiExecutor;
    }

    [HttpGet("login")]
    public IActionResult Login()
    {
        return View(new UserLogin());
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLogin userLogin)
    {
        if (!ModelState.IsValid) return View(userLogin);

        var success = await webApiExecutor.Login(userLogin);

        if (success)
        {
            return RedirectToAction("Index", "Home"); 
        }

        ModelState.AddModelError("", "Invalid username or password");
        return View(userLogin);
    }

    [HttpGet("logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}

