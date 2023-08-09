using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReservationSystem.Services.Interfaces;
using ReservationSystem.Web.ViewModels.Location;
using System.Security.Claims;
using static ReservationSystem.Common.GeneralApplicationConstants;

namespace ReservationSystem.Web.Controllers;

[Authorize]
public class LocationController : Controller
{
    private readonly ILocationService locationService;
    public LocationController(ILocationService locationService)
    {
        this.locationService = locationService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Details(int Id)
    {
        string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
        LocationDetailsViewModel model = await locationService.GetLocationDetailsAsync(Id, userId);

        if (model != null)
        {
            model.ReviewForm = new ReviewFormViewModel { LocationId = Id };
            IEnumerable<ReviewViewModel> reviews = await locationService.GetReviewsForLocationAsync(Id);
            ViewBag.Reviews = reviews; // Pass reviews to the view
            return View(model);
        }
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult AddReview(int locationId)
    {
        var model = new ReviewFormViewModel
        {
            LocationId = locationId
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> AddReview(ReviewFormViewModel model)
    {
        string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (await locationService.UserHasReviewedLocationAsync(userId, model.LocationId))
        {
            foreach (var key in ModelState.Keys)
            {
                ModelState[key].Errors.Clear();
            }
            ModelState.AddModelError(string.Empty, "You have already reviewed this location.");
        }
        else if (ModelState.IsValid)
        {
            await locationService.AddReviewAsync(model, userId);

            return RedirectToAction("Details", new { id = model.LocationId });
        }

        LocationDetailsViewModel detailsModel = await locationService.GetLocationDetailsAsync(model.LocationId, userId);
        detailsModel.ReviewForm = model;

        ViewBag.Reviews = await locationService.GetReviewsForLocationAsync(model.LocationId);

        return View("Details", detailsModel);
    }
}
