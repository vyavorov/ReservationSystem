using Microsoft.AspNetCore.Mvc;
using ReservationSystem.Services.Interfaces;
using ReservationSystem.Web.ViewModels.Home;

namespace ReservationSystem.Web.Areas.Admin.Controllers
{
    public class HomeController : BaseAdminController
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
    }
}
