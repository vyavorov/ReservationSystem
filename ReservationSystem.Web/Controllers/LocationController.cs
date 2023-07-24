using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ReservationSystem.Services.Interfaces;
using ReservationSystem.Web.ViewModels.Location;

namespace ReservationSystem.Web.Controllers
{
    [Authorize]
    public class LocationController : Controller
    {
        private readonly ILocationService locationService;
        public LocationController(ILocationService locationService)
        {
            this.locationService = locationService;
        }
        //TODO: THE BELOW SHOULD BE AVAILABLE FOR ADMINS ONLY
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(LocationFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            try
            {
                await this.locationService.AddLocationAsync(model);
            }
            catch (Exception)
            {
                this.ModelState.AddModelError(string.Empty, "Unexpected error occured while trying to add your location. Please try again later or contact administrator.");
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int Id)
        {
            LocationDetailsViewModel model = await locationService.GetLocationDetailsAsync(Id);

            if (model != null)
            {
                return View(model);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            LocationFormViewModel locationFormModel = await locationService.EditFormByIdAsync(id);
            return View(locationFormModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, LocationFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                await locationService.EditLocationByIdAsync(id, model);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            LocationDeleteViewModel locationDeleteViewModel = await locationService.DeleteFormByIdAsync(id);
            if (locationDeleteViewModel != null)
            {
                return View(locationDeleteViewModel);
            }
            return RedirectToAction("Index", "Home");
        }
        //TODO: MANIPULATE TRY/CATCH EVERYWHERE
        [HttpPost]
        public async Task<IActionResult> Delete(int id, LocationDeleteViewModel locationDeleteViewModel)
        {
            try
            {
                await locationService.DeleteLocationByIdAsync(id, locationDeleteViewModel);
            }
            catch (Exception)
            {
                throw;
            }

            return RedirectToAction("Index","Home");
        }
    }
}
