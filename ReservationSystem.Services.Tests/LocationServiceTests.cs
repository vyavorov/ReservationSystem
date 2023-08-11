using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using ReservationSystem.Data;
using ReservationSystem.Services.Interfaces;
using ReservationSystem.Web.ViewModels.Location;

namespace ReservationSystem.Services.Tests
{
    public class LocationServiceTests
    {
        private DbContextOptions<ReservationDbContext> dbOptions;
        private ReservationDbContext dbContext;
        private ILocationService locationService;

        [SetUp]
        public void SetUp()
        {
            this.dbOptions = new DbContextOptionsBuilder<ReservationDbContext>()
                .UseInMemoryDatabase("LocationInMemory" + Guid.NewGuid().ToString())
                .Options;

            this.dbContext = new ReservationDbContext(this.dbOptions);

            this.locationService = new LocationService(dbContext);

            //SeedDatabase(dbContext, locationService);
        }

        [TearDown]
        public void TearDown()
        {
            this.dbContext.Dispose();
        }

        [Test]
        public void LocationsCountShouldReturnOneWhenLocationIsAdded()
        {
            // Arrange
            var model = new LocationFormViewModel()
            {
                Address = "Pernik location address",
                Description = "Pernik location description",
                Capacity = 40,
                ImageUrl = "https://bashhub.bg/wp-content/uploads/2021/10/PERNIK-3.png",
                Name = "Pernik",
                PricePerDay = 35,
            };

            // Act
            locationService.AddLocationAsync(model).Wait();  // Using Wait to make it synchronous for the test

            int locationsCount = this.dbContext.Locations.Count();

            // Assert
            Assert.That(locationsCount, Is.EqualTo(1));
        }


        [Test]
        public async Task EditLocationByIdAsync_ShouldUpdateLocationProperties()
        {
            await DatabaseSeeder.SeedDatabase(dbContext, locationService);
            // Arrange
            var originalLocation = await dbContext.Locations.FirstOrDefaultAsync(l => l.Name == "Location1");
            Assert.IsNotNull(originalLocation);  // Ensure the location exists

            var model = new LocationFormViewModel
            {
                Address = "Updated Address",
                Description = "Updated Description",
                Capacity = 45,
                ImageUrl = "https://example.com/updated.png",
                Name = "Updated Location Name",
                PricePerDay = 40,
            };

            // Act
            await locationService.EditLocationByIdAsync(originalLocation.Id, model);

            var updatedLocation = await dbContext.Locations.FindAsync(originalLocation.Id);

            // Assert
            Assert.That(updatedLocation.Address, Is.EqualTo(model.Address));
            Assert.That(updatedLocation.Name, Is.EqualTo(model.Name));
        }

        [Test]
        public async Task EditFormByIdAsync_ShouldReturnCorrectFormViewModelForGivenId()
        {
            await DatabaseSeeder.SeedDatabase(dbContext, locationService);
            // Arrange
            var originalLocation = await dbContext.Locations.FirstOrDefaultAsync(l => l.Name == "Location1");
            Assert.IsNotNull(originalLocation);  // Ensure the location exists

            // Act
            var result = await locationService.EditFormByIdAsync(originalLocation.Id);

            // Assert
            Assert.That(result.Address, Is.EqualTo(originalLocation.Address));
            Assert.That(result.Name, Is.EqualTo(originalLocation.Name));
        }

        [Test]
        public async Task DeleteFormByIdAsync_ShouldReturnCorrectDeleteViewModelForGivenId()
        {
            await DatabaseSeeder.SeedDatabase(dbContext, locationService);
            // Arrange
            var originalLocation = await dbContext.Locations.FirstOrDefaultAsync(l => l.Name == "Location1");
            Assert.IsNotNull(originalLocation);  // Ensure the location exists

            // Act
            var result = await locationService.DeleteFormByIdAsync(originalLocation.Id);

            // Assert
            Assert.That(result.Address, Is.EqualTo(originalLocation.Address));
            Assert.That(result.Name, Is.EqualTo(originalLocation.Name));
        }

        [Test]
        public async Task DeleteLocationByIdAsync_ShouldMarkLocationAsInactive()
        {
            await DatabaseSeeder.SeedDatabase(dbContext, locationService);
            // Arrange
            var originalLocation = await dbContext.Locations.FirstOrDefaultAsync(l => l.Name == "Location1");
            Assert.IsNotNull(originalLocation);  // Ensure the location exists

            // Act
            await locationService.DeleteLocationByIdAsync(originalLocation.Id, new LocationDeleteViewModel());

            var locationAfterDeletion = await dbContext.Locations.FindAsync(originalLocation.Id);

            // Assert
            Assert.IsFalse(locationAfterDeletion.IsActive);
        }

        [Test]
        public async Task AddReviewAsync_ShouldAddReviewToDatabase()
        {
            await DatabaseSeeder.SeedDatabase(dbContext, locationService);
            // Arrange
            var knownUser = await dbContext.Users.FirstOrDefaultAsync(u => u.UserName == "testuser");
            Assert.IsNotNull(knownUser, "Known test user should be seeded in the database.");

            var location = await dbContext.Locations.FirstOrDefaultAsync(l => l.Name == "Location1");

            var model = new ReviewFormViewModel
            {
                Comment = "Great location!",
                LocationId = location.Id
            };

            // Act
            await locationService.AddReviewAsync(model, knownUser.Id.ToString());

            var review = await dbContext.Reviews.FirstOrDefaultAsync(r => r.UserId == knownUser.Id && r.LocationId == location.Id);

            // Assert
            Assert.IsNotNull(review);
            Assert.That(review.Comment, Is.EqualTo(model.Comment));
        }
    }
}