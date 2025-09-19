namespace EmployeeWebApp.Controllers;

[SessionAuthorize]
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
        if (ModelState.IsValid)
        {
            var response = await executor.CreateClient<string>(createClientDTO);
            if (response != null)
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


    //Update Client
    [HttpGet]
    public async Task<IActionResult> UpdateClient(int id)
    {
        var titles = await GetListOfTitles();
        var types = await GetListOfTypes();
        var client = await GetClient(id);

        ViewBag.Titles = titles;
        ViewBag.ClientTypes = types;
        ViewBag.Client = client;

        if(client == null)
        {
            return NotFound();
        }

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> UpdateClient([FromForm] UpdateClientDTO dto)
    {
        if(ModelState.IsValid)
        {
            var response = await executor.UpdateClient<string>(dto);
            if (response != null)
            {
                return RedirectToAction(nameof(Index));
            }
        }

        var titles = await GetListOfTitles();
        var types = await GetListOfTypes();
        var client = await GetClient(dto.Id);

        ViewBag.Titles = titles;
        ViewBag.ClientTypes = types;
        ViewBag.Client = client;

        return View(dto);
    }


    //Update by client Type
    [HttpGet]
    public async Task<IActionResult> UpdateClientsByTypes()
    {
        var types = await GetListOfTypes();
        ViewBag.ClientTypes = types;

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> UpdateClientsByTypes(UpdateByClientType dto)
    {
        if (ModelState.IsValid)
        {
            await executor.UpdateClientType<UpdateByClientType>(dto);
            return RedirectToAction(nameof(Index));
        }
        return View(dto);
    }

    //Delete Client
    [HttpGet]
    public async Task<IActionResult> DeleteClient(int id)
    {
        await executor.DeleteClient(id);
        return RedirectToAction(nameof(Index));
    }

    //get client
    private async Task<ClientDTO> GetClient(int id)
    {
        var Client =  await executor.GetClientById<ClientDTO>(id);
        if (Client == null) return null;
        
        return Client;
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
