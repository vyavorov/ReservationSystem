using ReservationSystem.Web.ViewModels.Home;
using ReservationSystem.Web.ViewModels.Location;

namespace ReservationSystem.Services.Interfaces;

public interface ILocationService
{
    Task<IEnumerable<IndexViewModel>> GetAllLocationsAsync();
    Task AddLocationAsync(LocationFormViewModel model);
    Task<LocationDetailsViewModel> GetLocationDetailsAsync(int id, string userId);
    Task<LocationFormViewModel> EditFormByIdAsync(int id);
    Task EditLocationByIdAsync(int id,  LocationFormViewModel model);
    Task<LocationDeleteViewModel> DeleteFormByIdAsync(int id);
    Task DeleteLocationByIdAsync(int id, LocationDeleteViewModel model);
    Task AddReviewAsync(ReviewFormViewModel model, string userId);
    Task<IEnumerable<ReviewViewModel>> GetReviewsForLocationAsync(int locationId);
    Task<bool> UserHasReviewedLocationAsync(string userId, int locationId);


}
