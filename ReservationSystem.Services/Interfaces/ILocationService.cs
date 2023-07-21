using ReservationSystem.Web.ViewModels.Home;
using ReservationSystem.Web.ViewModels.Location;

namespace ReservationSystem.Services.Interfaces;

public interface ILocationService
{
    Task<IEnumerable<IndexViewModel>> GetAllLocationsAsync();
    Task AddLocationAsync(LocationFormModel model);
}
