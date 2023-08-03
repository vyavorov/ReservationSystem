using Microsoft.AspNetCore.Mvc;
using ReservationSystem.Services.Interfaces;
using ReservationSystem.Web.ViewModels.Home;
using System.Diagnostics;

namespace ReservationSystem.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILocationService locationService;
    public HomeController(ILocationService locationService)
    {
        this.locationService = locationService;
    }
    
    public async Task<IActionResult> Index()
    {
        IEnumerable<IndexViewModel> locations = await locationService.GetAllLocationsAsync();

        return View(locations);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}