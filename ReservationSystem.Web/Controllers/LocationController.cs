﻿using Microsoft.AspNetCore.Mvc;

namespace ReservationSystem.Web.Controllers
{
    public class LocationController : Controller
    {
        public async Task<IActionResult> Add()
        {
            return View();
        }
    }
}
