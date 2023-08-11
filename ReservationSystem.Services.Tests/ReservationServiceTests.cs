using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using ReservationSystem.Data;
using ReservationSystem.Data.Models;
using ReservationSystem.Services.Interfaces;
using ReservationSystem.Web.ViewModels.Reservation;
using System.Linq;

namespace ReservationSystem.Services.Tests
{
    public class ReservationServiceTests
    {
        private ReservationDbContext dbContext;
        private IReservationService reservationService;
        private ILocationService locationService;

        [SetUp]
        public async Task SetUp()
        {
            var options = new DbContextOptionsBuilder<ReservationDbContext>()
                .UseInMemoryDatabase(databaseName: $"ReservationInMemory{Guid.NewGuid()}")
                .Options;

            dbContext = new ReservationDbContext(options);
            reservationService = new ReservationService(dbContext);
            locationService = new LocationService(dbContext);

            await DatabaseSeeder.SeedDatabaseForReservationTests(dbContext, locationService);
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }

        [Test]
        public async Task CreateReservationAsync_ValidData_CreatesReservation()
        {
            var location = await dbContext.Locations.FindAsync(1);
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.UserName == "testuser");

            var model = new ReservationFormViewModel
            {
                LocationId = location.Id,
                From = DateTime.UtcNow,
                To = DateTime.UtcNow,
                CustomersCount = 1,
                CreatedOn = DateTime.UtcNow,
                PhoneNumber = "0888888888",
                UserId = user.Id
            };

            await reservationService.CreateReservationAsync(model);

            Assert.That(dbContext.Reservations.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task CreateReservationAsync_InvalidPromoCode_ThrowsArgumentException()
        {
            var location = await dbContext.Locations.FindAsync(1);
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.UserName == "testuser");

            var model = new ReservationFormViewModel
            {
                PromoCode = "INVALID_CODE",
                LocationId = location.Id,
                From = DateTime.UtcNow,
                To = DateTime.UtcNow,
                CustomersCount = 1,
                CreatedOn = DateTime.UtcNow,
                PhoneNumber = "0888888888",
                UserId = user.Id
            };

            Assert.ThrowsAsync<ArgumentException>(() => reservationService.CreateReservationAsync(model));
        }

        [Test]
        public async Task GetAllReservationsForUserAsync_ValidUserId_ReturnsReservations()
        {
            var reservations = await reservationService.GetAllReservationsForUserASync("5aaa9dfd-b9c3-44e6-8edc-c802f9cff17e");

            Assert.That(reservations.Count, Is.EqualTo(1));
        }

        [Test]
        public void GetReservationModelByIdAsync_InvalidId_ThrowsArgumentException()
        {
            Assert.ThrowsAsync<ArgumentException>(() => reservationService.GetReservationModelByIdAsync("InvalidId"));
        }

        [Test]
        public async Task ReservationTotalPriceShouldConsiderDiscount()
        {
            PromoCode promoCode = await this.dbContext.PromoCodes.FirstAsync();
            ReservationFormViewModel reservationModel = new ReservationFormViewModel()
            {
                CreatedOn = DateTime.UtcNow,
                PromoCode = promoCode.Name,
                CustomersCount= 1,
                From = DateTime.UtcNow,
                To = DateTime.UtcNow,
                LocationId = 1,
                PhoneNumber = "0888888888",
            };
            await reservationService.CreateReservationAsync(reservationModel);

            var reservation = await this.dbContext.Reservations.LastAsync();

            Assert.That(reservation.TotalPrice, Is.EqualTo(8.75));
        }
    }
}