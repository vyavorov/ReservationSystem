using ReservationSystem.Web.ViewModels.Home;

namespace ReservationSystem.Services.Interfaces;

public interface ILocationService
{
    Task<IEnumerable<IndexViewModel>> GetAllLocationsAsync();
}
