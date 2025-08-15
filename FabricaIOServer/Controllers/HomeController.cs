using System.Diagnostics;
using FabricaIOServer.Models;
using Microsoft.AspNetCore.Mvc;

namespace FabricaIOServer.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

	public IActionResult Index()
	{
		return Redirect("https://github.com/FabricaIO");
		
		// Server is currently only used for API access. People looking for information are redirected to the GitHub organization for now.
        //return View();
	}

	// Not implemented yet, and may be unnecessary
    // public IActionResult Privacy()
	// {
	//     return View();
	// }

	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
