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
            PricePerDay = model.PricePerDay,
            Description = model.Description,
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
                PricePerDay = l.PricePerDay,
                Address = l.Address,
            })
            .ToArrayAsync();

        return locations;
    }

    public async Task<LocationFormModel> EditFormByIdAsync(int id)
    {
        Location? location = await context.Locations.FirstOrDefaultAsync(l => l.Id == id);
        LocationFormModel? locationFormModel = null;
        if (location != null)
        {
            locationFormModel = new LocationFormModel()
            {
                Address = location.Address,
                Capacity = location.Capacity,
                ImageUrl = location.ImageUrl,
                PricePerDay = location.PricePerDay,
                Description = location.Description,
                Name = location.Name,
            };
        }
        return locationFormModel;
    }

    public async Task<LocationDetailsViewModel> GetLocationDetailsAsync(int id)
    {
        Location? location = await context.Locations.FirstOrDefaultAsync(l => l.Id == id);
        LocationDetailsViewModel? locationDetailsViewModel = null;
        if (location != null)
        {
            locationDetailsViewModel = new LocationDetailsViewModel()
            {
                Id = location.Id,
                Address = location.Address,
                Capacity = location.Capacity,
                ImageUrl = location.ImageUrl,
                PricePerDay = location.PricePerDay,
                Description = location.Description,
                Name = location.Name
            };
        }
        return locationDetailsViewModel;
    }

    public async Task EditLocationByIdAsync(int id, LocationFormModel model)
    {
        Location? location = await context.Locations.FirstOrDefaultAsync(l => l.Id == id);
        if (location != null)
        {
            location.Address = model.Address;
            location.Capacity = model.Capacity;
            location.ImageUrl = model.ImageUrl;
            location.Address = model.Address;
            location.PricePerDay = model.PricePerDay;
            location.Description = model.Description;
            location.Name = model.Name;
            await context.SaveChangesAsync();
        }
    }
}
