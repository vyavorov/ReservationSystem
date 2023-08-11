using Microsoft.AspNetCore.Mvc;
using ReservationSystem.Data.Models;
using ReservationSystem.Services.Interfaces;
using ReservationSystem.Web.ViewModels.Reservation;
using static ReservationSystem.Common.GeneralApplicationConstants;

namespace ReservationSystem.Web.Areas.Admin.Controllers;

public class ReservationController : BaseAdminController
{
    private readonly IReservationService reservationService;
    public ReservationController(IReservationService reservationService)
    {
        this.reservationService = reservationService;
    }

    [HttpGet]
    public async Task<IActionResult> All(int? locationId = null)
    {
        ICollection<Location> locations = await reservationService.GetAllLocationsAsync();
        ICollection<AllReservationsViewModel> reservations = await reservationService.GetAllReservationsASync(locationId);
        return View(new AllReservationsWithLocationsViewModel
        {
            Reservations = reservations,
            Locations = locations
        });
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string Id)
    {
        ReservationFormViewModel reservation;
        try
        {
            reservation = await reservationService.GetReservationModelByIdAsync(Id);
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return RedirectToAction("All", "Reservation", new { Area = AdminAreaName });
        }
        return View(reservation);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(string Id, ReservationFormViewModel model)
    {
        try
        {
            await reservationService.EditReservationAsync(Id, model);
        }
        catch (ArgumentException ex)
        {
            model.Equipments = await reservationService.GetAllEquipmentsAsync();
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
        return RedirectToAction("All", "Reservation", new { Area = AdminAreaName });
    }

    [HttpGet]
    public async Task<IActionResult> Delete(string Id)
    {

        ReservationFormViewModel reservation;
        try
        {
            reservation = await reservationService.GetReservationModelByIdAsync(Id);
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View();
        }
        return View(reservation);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(string Id, ReservationFormViewModel model)
    {
        try
        {
            await reservationService.DeleteReservationAsync(Id, model);
        }
        catch (Exception)
        {
            return RedirectToAction("All", "Reservation", new { Area = AdminAreaName });
        }
        return RedirectToAction("All", "Reservation", new { Area = AdminAreaName });
    }
}
