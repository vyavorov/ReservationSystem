using ReservationSystem.Data;
using ReservationSystem.Data.Models;
using ReservationSystem.Services.Interfaces;
using ReservationSystem.Web.ViewModels.Location;

public static class DatabaseSeeder
{
    public static async Task SeedDatabaseForLocationTests(ReservationDbContext context, ILocationService locationService)
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


    public static async Task SeedDatabaseForReservationTests(ReservationDbContext context, ILocationService locationService)
    {
        var location1 = new Location()
        {
            Id = 1,
            Address = "Location1 Address",
            Description = "Description for Location1",
            Capacity = 40,
            ImageUrl = "https://example.com/location1.png",
            Name = "Location1",
            PricePerDay = 35,
        };

        var location2 = new Location()
        {
            Id = 2,
            Address = "Location2 Address",
            Description = "Description for Location2",
            Capacity = 50,
            ImageUrl = "https://example.com/location2.png",
            Name = "Location2",
            PricePerDay = 45,
        };

        var user = new ApplicationUser
        {
            Id = Guid.Parse("5aaa9dfd-b9c3-44e6-8edc-c802f9cff17e"),
            UserName = "testuser",
            Email = "testuser@example.com",
            FirstName = "Test",
            LastName = "Test",
        };

        Reservation reservation = new Reservation()
        {
            CreatedOn = DateTime.UtcNow,
            CustomersCount = 1,
            From = DateTime.UtcNow,
            To = DateTime.UtcNow,
            LocationId = location1.Id,
            PhoneNumber = "08888888888",
            UserId = user.Id,
        };

        PromoCode promocode = new PromoCode()
        {
            Name = "internal",
            Discount = 75
        };

        await context.Users.AddAsync(user);

        await context.Reservations.AddAsync(reservation);

        await context.PromoCodes.AddAsync(promocode);

        await context.Locations.AddAsync(location1);
        await context.Locations.AddAsync(location2);

        await context.SaveChangesAsync();
    }
}