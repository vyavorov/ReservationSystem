using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ReservationSystem.Web.Controllers
{
    [Authorize]
    public class ReservationController : Controller
    {
        public async Task<IActionResult> Mine()
        {
            return View();
        }
    }
}
