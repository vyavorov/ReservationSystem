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
        public async Task<IActionResult> Book(int locationId)
        {
            ReservationFormViewModel viewModel = new ReservationFormViewModel()
            {
                Equipments = await this.reservationService
                    .GetAllEquipmentsAsync()
                    
            };

            return View(viewModel);
        }
    }
}
