using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FabricaIOServer.Models;
using System.Threading.Tasks;
using FabricaIOServer.FabricaIOLib;
using Newtonsoft;
using Newtonsoft.Json;

namespace FabricaIOServer.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        GitHubAPI api = new("Fabrica-IO-Server");
        IReadOnlyList<string> jsons = await api.getLibraryJsons(await api.getRepos());
        foreach (string json in jsons)
        {
            FabricaIODevice? device = JsonConvert.DeserializeObject<FabricaIODevice>(json);
            if (device?.fabricaio != null)
            {
                Console.WriteLine(device?.name);
            }
        }
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
