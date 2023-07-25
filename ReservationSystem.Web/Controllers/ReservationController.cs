using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReservationSystem.Services.Interfaces;
using ReservationSystem.Web.ViewModels.Reservation;

namespace ReservationSystem.Web.Controllers
{
    [Authorize]
    public class ReservationController : Controller
    {
        private readonly IReservationService reservationService;
        public ReservationController(IReservationService reservationService)
        {
            this.reservationService = reservationService;
        }

        [HttpGet]
        public async Task<IActionResult> Book(int locationId)
        {
            ReservationFormViewModel viewModel = new ReservationFormViewModel()
            {
                From = DateTime.Today,
                To = DateTime.Today,
                Equipments = await this.reservationService
                    .GetAllEquipmentsAsync(),
                LocationId = locationId
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Book(int Id, ReservationFormViewModel model)
        {
            model.LocationId = Id;

            return RedirectToAction("Index","Home");
        }
    }
}
