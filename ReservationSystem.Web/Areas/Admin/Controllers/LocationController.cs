using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReservationSystem.Services.Interfaces;
using ReservationSystem.Web.ViewModels.Location;
using static ReservationSystem.Common.GeneralApplicationConstants;

namespace ReservationSystem.Web.Areas.Admin.Controllers
{
    public class LocationController : BaseAdminController
    {

        private readonly ILocationService locationService;
        public LocationController(ILocationService locationService)
        {
            this.locationService = locationService;
        }

        [HttpGet]
        [Authorize(Roles = AdminRoleName)]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = AdminRoleName)]
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
            return RedirectToAction("Index", "Home", new { Area = AdminAreaName });
        }

        [HttpGet]
        [Authorize(Roles = AdminRoleName)]
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
        [Authorize(Roles = AdminRoleName)]
        public async Task<IActionResult> Edit(int id, LocationFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                await locationService.EditLocationByIdAsync(id, model);
            }
            return RedirectToAction("Index", "Home", new { Area = AdminAreaName });
        }

        [HttpGet]
        [Authorize(Roles = AdminRoleName)]
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
        [Authorize(Roles = AdminRoleName)]
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

            return RedirectToAction("Index", "Home", new { Area = AdminAreaName });
        }
    }
}
