using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ReservationSystem.Web.Controllers
{
    [Authorize]
    public class LocationController : Controller
    {
        //TODO: THE BELOW SHOULD BE AVAILABLE FOR ADMINS ONLY
        public async Task<IActionResult> Add()
        {
            return View();
        }
    }
}
