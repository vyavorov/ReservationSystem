using Microsoft.EntityFrameworkCore;
using ReservationSystem.Data;
using ReservationSystem.Data.Models;
using ReservationSystem.Services.Interfaces;
using ReservationSystem.Web.ViewModels.Home;
using ReservationSystem.Web.ViewModels.Location;

namespace ReservationSystem.Services;

public class LocationService : ILocationService
{
    private readonly ReservationDbContext context;
    public LocationService(ReservationDbContext context)
    {
        this.context = context;
    }

    public async Task AddLocationAsync(LocationFormModel model)
    {
        Location location = new Location()
        {
            Name = model.Name,
            Address = model.Address,
            Capacity = model.Capacity,
            ImageUrl = model.ImageUrl,
            PricePerDay = model.PricePerDay
        };
        await context.Locations.AddAsync(location);
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<IndexViewModel>> GetAllLocationsAsync()
    {
        IEnumerable<IndexViewModel> locations = await context
            .Locations
            .AsNoTracking()
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
