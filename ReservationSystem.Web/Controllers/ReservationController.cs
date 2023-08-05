using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReservationSystem.Data.Models;
using ReservationSystem.Services.Interfaces;
using ReservationSystem.Web.ViewModels.Reservation;
using System.Collections.Specialized;
using System.Net;
using System.Security.Claims;

namespace ReservationSystem.Web.Controllers;

[Authorize]
public class ReservationController : Controller
{
    private readonly IReservationService reservationService;
    public ReservationController(IReservationService reservationService)
    {
        this.reservationService = reservationService;
    }

    [HttpGet]
    public async Task<IActionResult> Book(int Id)
    {
        ReservationFormViewModel viewModel = new ReservationFormViewModel()
        {
            From = DateTime.Today,
            To = DateTime.Today,
            Equipments = await this.reservationService
                .GetAllEquipmentsAsync(),
            LocationId = Id,
            PricePerDay = await reservationService.GetPricePerDayByLocation(Id)
        };

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Book(int Id, ReservationFormViewModel model)
    {
        model.LocationId = Id;
        try
        {
            await reservationService.CreateReservationAsync(model);
        }
        catch (ArgumentException ex)
        {
            model.Equipments = await reservationService.GetAllEquipmentsAsync();
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }

        return RedirectToAction("Mine", "Reservation");
    }

    [HttpGet]
    public async Task<IActionResult> Mine()
    {
        string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
        ICollection<ReservationFormViewModel> reservations = await reservationService
            .GetAllReservationsForUserASync(userId);
        return View(reservations);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string Id)
    {
        ReservationFormViewModel reservation;
        try
        {
            reservation = await reservationService.GetReservationModelToByIdAsync(Id);
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return RedirectToAction("Mine", "Reservation");
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
        return RedirectToAction("Mine", "Reservation");
    }

    [HttpGet]
    public async Task<IActionResult> Delete(string Id)
    {

        ReservationFormViewModel reservation;
        try
        {
            reservation = await reservationService.GetReservationModelToByIdAsync(Id);
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
            return RedirectToAction("Mine", "Reservation");
        }
        return RedirectToAction("Mine", "Reservation");
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
}
