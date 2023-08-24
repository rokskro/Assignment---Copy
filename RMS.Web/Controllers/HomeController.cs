using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RMS.Web.Models;

namespace RMS.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult About()
    {
        var about = new AboutViewModel {
            Title = "About Us",
            Message = "Our mission is to develop great recipes for happy eating",
            Formed = new DateTime(2023,04,6)
        };
        return View(about);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
