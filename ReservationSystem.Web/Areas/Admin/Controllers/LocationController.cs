using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ReservationSystem.Services.Interfaces;
using ReservationSystem.Web.ViewModels.Location;
using static ReservationSystem.Common.GeneralApplicationConstants;

namespace ReservationSystem.Web.Areas.Admin.Controllers
{
    public class LocationController : BaseAdminController
    {

        private readonly ILocationService locationService;
        private readonly IMemoryCache memoryCache;
        public LocationController(ILocationService locationService, IMemoryCache memoryCache)
        {
            this.locationService = locationService;
            this.memoryCache = memoryCache;
        }

        [HttpGet]
        public IActionResult Add()
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
                this.memoryCache.Remove(LocationsCacheKey);
            }
            catch (Exception)
            {
                this.ModelState.AddModelError(string.Empty, "Unexpected error occured while trying to add your location. Please try again later or contact administrator.");
            }
            return RedirectToAction("Index", "Home", new { Area = AdminAreaName });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            LocationFormViewModel locationFormModel = await locationService.EditFormByIdAsync(id);
            if (locationFormModel != null)
            {
                return View(locationFormModel);
            }
            return RedirectToAction("Index", "Home", new { Area = AdminAreaName });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, LocationFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                await locationService.EditLocationByIdAsync(id, model);
                this.memoryCache.Remove(LocationsCacheKey);
            }
            return RedirectToAction("Index", "Home", new { Area = AdminAreaName });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            LocationDeleteViewModel locationDeleteViewModel = await locationService.DeleteFormByIdAsync(id);
            if (locationDeleteViewModel != null)
            {
                return View(locationDeleteViewModel);
            }
            return RedirectToAction("Index", "Home", new { Area = AdminAreaName });
        }
        //TODO: MANIPULATE TRY/CATCH EVERYWHERE
        [HttpPost]
        public async Task<IActionResult> Delete(int id, LocationDeleteViewModel locationDeleteViewModel)
        {
            try
            {
                await locationService.DeleteLocationByIdAsync(id, locationDeleteViewModel);
                this.memoryCache.Remove(LocationsCacheKey);
            }
            catch (Exception)
            {
                throw;
            }

            return RedirectToAction("Index", "Home", new { Area = AdminAreaName });
        }
    }
}
