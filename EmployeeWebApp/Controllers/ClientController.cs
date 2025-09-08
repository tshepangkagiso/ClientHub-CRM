

namespace EmployeeWebApp.Controllers;

public class ClientController : Controller
{
    private readonly IWebApiExecutor executor;
    public ClientController(IWebApiExecutor executor)
    {
        this.executor = executor;
    }

    //Get All Clients
    public async Task<IActionResult> Index()
    {
        var clients = await executor.GetAllClients<List<ClientDTO>>();
        return View(clients);
    }
        
    //Create Client
    public async Task<IActionResult> CreateClient()
    {
        var titles = await GetListOfTitles();
        var types = await GetListOfTypes();
        ViewBag.Titles = titles;
        ViewBag.ClientTypes = types;

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateClient([FromForm] CreateClientDTO createClientDTO)
    {
        if(ModelState.IsValid)
        {
            var response = await executor.CreateClient(createClientDTO);
            if(response != null)
            {
                return RedirectToAction(nameof(Index));
            }
        }

        var titles = await GetListOfTitles();
        var types = await GetListOfTypes();

        ViewBag.Titles = titles;
        ViewBag.ClientTypes = types;

        return View(createClientDTO);

    }



    //get all titles
    private async Task<List<string>> GetListOfTitles()
    {
        var titles = await executor.GetTitles<List<string>>();
        if(titles == null ||  titles.Count == 0)
        {
            return new List<string>();
        }
        else
        {
            return titles;
        }
    }
    //get all client types
    private async Task<List<string>> GetListOfTypes()
    {
        var types = await executor.GetTypes<List<string>>();
        if (types == null || types.Count == 0)
        {
            return new List<string>();
        }
        else
        {
            return types;
        }
    }
}
