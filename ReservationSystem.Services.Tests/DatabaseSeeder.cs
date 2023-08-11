using ReservationSystem.Data;
using ReservationSystem.Data.Models;
using ReservationSystem.Services.Interfaces;
using ReservationSystem.Web.ViewModels.Location;

public static class DatabaseSeeder
{
    public static async Task SeedDatabase(ReservationDbContext context, ILocationService locationService)
    {
        var location1 = new LocationFormViewModel()
        {
            Address = "Location1 Address",
            Description = "Description for Location1",
            Capacity = 40,
            ImageUrl = "https://example.com/location1.png",
            Name = "Location1",
            PricePerDay = 35,
        };

        var location2 = new LocationFormViewModel()
        {
            Address = "Location2 Address",
            Description = "Description for Location2",
            Capacity = 50,
            ImageUrl = "https://example.com/location2.png",
            Name = "Location2",
            PricePerDay = 45,
        };

        // Seed a user:
        var user = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            UserName = "testuser",
            Email = "testuser@example.com",
            FirstName = "Test",
            LastName = "Test",
        };

        context.Users.Add(user);

        await locationService.AddLocationAsync(location1);
        await locationService.AddLocationAsync(location2);

        await context.SaveChangesAsync();
    }
}