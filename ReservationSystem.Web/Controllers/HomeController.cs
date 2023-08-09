﻿using Microsoft.AspNetCore.Mvc;
using ReservationSystem.Services.Interfaces;
using ReservationSystem.Web.ViewModels.Home;
using static ReservationSystem.Common.GeneralApplicationConstants;

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
        if (this.User.IsInRole(AdminRoleName))
        {
            return RedirectToAction("Index", "Home", new { Area = AdminAreaName });
        }

        IEnumerable<IndexViewModel> locations = await locationService.GetAllLocationsAsync();

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