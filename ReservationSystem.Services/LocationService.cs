using Microsoft.EntityFrameworkCore;
using ReservationSystem.Data;
using ReservationSystem.Services.Interfaces;
using ReservationSystem.Web.ViewModels.Home;

namespace ReservationSystem.Services;

public class LocationService : ILocationService
{
    private readonly ReservationDbContext context;
    public LocationService(ReservationDbContext context)
    {
        this.context = context;
    }

    public async Task<IEnumerable<IndexViewModel>> GetAllLocationsAsync()
    {
        IEnumerable<IndexViewModel> locations = await context
            .Locations
            .Select(l => new IndexViewModel
            {
                Id = l.Id,
                Name = l.Name,
                ImageUrl = l.ImageUrl,
            })
            .ToArrayAsync();

        return locations;
    }
}
