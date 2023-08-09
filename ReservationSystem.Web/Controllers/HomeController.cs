using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ReservationSystem.Services.Interfaces;
using ReservationSystem.Web.ViewModels.Home;
using static ReservationSystem.Common.GeneralApplicationConstants;

namespace ReservationSystem.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILocationService locationService;
    private readonly IMemoryCache memoryCache;
    public HomeController(ILocationService locationService, IMemoryCache memoryCache)
    {
        this.locationService = locationService;
        this.memoryCache = memoryCache;
    }

    public async Task<IActionResult> Index()
    {
        if (this.User.IsInRole(AdminRoleName))
        {
            return RedirectToAction("Index", "Home", new { Area = AdminAreaName });
        }
        IEnumerable<IndexViewModel> locations = this.memoryCache.Get<IEnumerable<IndexViewModel>>(LocationsCacheKey);
        if (locations == null)
        {
            locations = await locationService.GetAllLocationsAsync();

            MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(LocationsCacheDurationMinutes));

            this.memoryCache.Set(LocationsCacheKey, locations, cacheOptions);
        }
        return View(locations);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error(int statusCode)
    {
        if (statusCode == 400 || statusCode == 404)
        {
            return this.View("Error404");
        }
        return View();
    }
}