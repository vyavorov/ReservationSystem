using Microsoft.EntityFrameworkCore;
using ReservationSystem.Data;
using ReservationSystem.Data.Models;
using ReservationSystem.Services.Interfaces;
using ReservationSystem.Web.ViewModels.Home;
using ReservationSystem.Web.ViewModels.Location;
using System.Security.Claims;

namespace ReservationSystem.Services;

public class LocationService : ILocationService
{
    private readonly ReservationDbContext context;
    public LocationService(ReservationDbContext context)
    {
        this.context = context;
    }

    public async Task AddLocationAsync(LocationFormViewModel model)
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
            .Where(l => l.IsActive)
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

    public async Task<LocationFormViewModel> EditFormByIdAsync(int id)
    {
        Location? location = await context.Locations.FirstOrDefaultAsync(l => l.Id == id);
        LocationFormViewModel? locationFormModel = null;
        if (location != null)
        {
            locationFormModel = new LocationFormViewModel()
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

    public async Task<LocationDetailsViewModel> GetLocationDetailsAsync(int id, string userId)
    {
        Location? location = await context.Locations.FirstOrDefaultAsync(l => l.Id == id);
        LocationDetailsViewModel? locationDetailsViewModel = null;
        if (location != null && location.IsActive)
        {
            locationDetailsViewModel = new LocationDetailsViewModel()
            {
                Id = location.Id,
                Address = location.Address,
                Capacity = location.Capacity,
                ImageUrl = location.ImageUrl,
                PricePerDay = location.PricePerDay,
                Description = location.Description,
                Name = location.Name,
                HasUserReviewed = await context.Reviews.AnyAsync(r => r.LocationId == id && r.UserId.ToString() == userId)
            };
        }
        return locationDetailsViewModel;
    }

    public async Task EditLocationByIdAsync(int id, LocationFormViewModel model)
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

    public async Task<LocationDeleteViewModel> DeleteFormByIdAsync(int id)
    {
        Location? location = await context.Locations.Where(l => l.IsActive).FirstOrDefaultAsync(l => l.Id ==id);
        LocationDeleteViewModel? locationDeleteViewModel = null;
        if (location != null)
        {
            locationDeleteViewModel = new LocationDeleteViewModel()
            {
                Address = location.Address,
                ImageUrl = location.ImageUrl,
                Name = location.Name
            };
        }
        return locationDeleteViewModel;
    }

    public async Task DeleteLocationByIdAsync(int id, LocationDeleteViewModel model)
    {
        Location? location = await context.Locations.Where(l => l.IsActive).FirstOrDefaultAsync(l => l.Id == id);
        if (location != null)
        {
            location.IsActive = false;
            await context.SaveChangesAsync();
        }
    }

    public async Task AddReviewAsync(ReviewFormViewModel model, string userId)
    {
        Review review = new Review
        {
            Comment = model.Comment,
            LocationId = model.LocationId,
            UserId = Guid.Parse(userId)
        };
        await context.Reviews.AddAsync(review);
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<ReviewViewModel>> GetReviewsForLocationAsync(int locationId)
    {
        var reviews = await context.Reviews
            .Where(r => r.LocationId == locationId)
            .Select(r => new ReviewViewModel { Comment = r.Comment, Username = r.User.UserName })
            .ToListAsync();

        return reviews;
    }

    public async Task<bool> UserHasReviewedLocationAsync(string userId, int locationId)
    {
        return await context.Reviews.AnyAsync(r => r.UserId.ToString() == userId && r.LocationId == locationId);
    }
}
