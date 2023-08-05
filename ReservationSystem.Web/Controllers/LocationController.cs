using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ReservationSystem.Services.Interfaces;
using ReservationSystem.Web.ViewModels.Location;
using System.Security.Claims;

namespace ReservationSystem.Web.Controllers;

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
        string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
        LocationDetailsViewModel model = await locationService.GetLocationDetailsAsync(Id, userId);
        model.ReviewForm = new ReviewFormViewModel { LocationId = Id };
        IEnumerable<ReviewViewModel> reviews = await locationService.GetReviewsForLocationAsync(Id);

        if (model != null)
        {
            ViewBag.Reviews = reviews; // Pass reviews to the view
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

        // Check if a review already exists for this user and location
        if (await locationService.UserHasReviewedLocationAsync(userId, model.LocationId))
        {
            // Clear all model-level errors
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

        // Load the details model including existing reviews
        LocationDetailsViewModel detailsModel = await locationService.GetLocationDetailsAsync(model.LocationId, userId);
        detailsModel.ReviewForm = model; // Attach the invalid form model back

        // Load existing reviews
        ViewBag.Reviews = await locationService.GetReviewsForLocationAsync(model.LocationId);

        return View("Details", detailsModel); // Here it returns to 'Details' view
    }
}
