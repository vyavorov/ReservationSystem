using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using ReservationSystem.Data;
using ReservationSystem.Data.Models;

namespace ReservationSystem.Services.Tests
{
    public class PromoCodeServiceTests
    {
        private DbContextOptions<ReservationDbContext> dbOptions;
        private ReservationDbContext dbContext;
        private PromoCodeService promoCodeService;

        [SetUp]
        public void SetUp()
        {
            this.dbOptions = new DbContextOptionsBuilder<ReservationDbContext>()
                .UseInMemoryDatabase(databaseName: "PromoCodeInMemory" + Guid.NewGuid().ToString())
                .Options;

            this.dbContext = new ReservationDbContext(this.dbOptions);

            this.promoCodeService = new PromoCodeService(dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            this.dbContext.Dispose();
        }

        [Test]
        public async Task CreatePromoCodeAsync_ShouldAddPromoCodeToDatabase()
        {
            // Arrange
            var promoCode = new PromoCode
            {
                Name = "TESTPROMO",
                Discount = 10
            };

            // Act
            await promoCodeService.CreatePromoCodeAsync(promoCode);

            // Assert
            var storedPromoCode = await dbContext.PromoCodes.FirstOrDefaultAsync(pc => pc.Name == "TESTPROMO");
            Assert.IsNotNull(storedPromoCode);
            Assert.That(storedPromoCode.Discount, Is.EqualTo(promoCode.Discount));
        }

        [Test]
        public async Task CreatePromoCodeAsync_ShouldThrowArgumentException_ForInvalidDiscount()
        {
            // Arrange
            var promoCode = new PromoCode
            {
                Name = "TESTPROMO",
                Discount = 110
            };

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(() => promoCodeService.CreatePromoCodeAsync(promoCode));
        }

        [Test]
        public async Task CreatePromoCodeAsync_ShouldThrowArgumentException_ForNullName()
        {
            // Arrange
            var promoCode = new PromoCode
            {
                Name = null,
                Discount = 10
            };

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(() => promoCodeService.CreatePromoCodeAsync(promoCode));
        }

        [Test]
        public async Task DeleteAsync_ShouldMarkPromoCodeAsInactive()
        {
            // Arrange
            var promoCode = new PromoCode
            {
                Name = "TESTPROMO",
                Discount = 10,
                IsActive = true
            };
            dbContext.PromoCodes.Add(promoCode);
            await dbContext.SaveChangesAsync();

            // Act
            await promoCodeService.DeleteAsync(promoCode.Id.ToString(), promoCode);

            // Assert
            var storedPromoCode = await dbContext.PromoCodes.FirstOrDefaultAsync(pc => pc.Id == promoCode.Id);
            Assert.IsFalse(storedPromoCode.IsActive);
        }

        [Test]
        public async Task DeleteAsync_ShouldThrowArgumentException_ForNonExistentPromoCode()
        {
            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(() => promoCodeService.DeleteAsync(Guid.NewGuid().ToString(), new PromoCode()));
        }

        [Test]
        public async Task GetPromoCodeByIdAsync_ShouldReturnPromoCode()
        {
            // Arrange
            var promoCode = new PromoCode
            {
                Name = "TESTPROMO",
                Discount = 10,
                IsActive = true
            };
            dbContext.PromoCodes.Add(promoCode);
            await dbContext.SaveChangesAsync();

            // Act
            var returnedPromoCode = await promoCodeService.GetPromoCodeByIdAsync(promoCode.Id.ToString());

            // Assert
            Assert.That(returnedPromoCode.Name, Is.EqualTo(promoCode.Name));
            Assert.That(returnedPromoCode.Discount, Is.EqualTo(promoCode.Discount));
        }

        //[Test]
        //public async Task GetPromoCodeByIdAsync_ShouldThrowArgumentException_ForNonExistentPromoCode()
        //{
        //    // Act & Assert
        //    Assert.ThrowsAsync<ArgumentException>(() => promoCodeService.GetPromoCodeByIdAsync(Guid.NewGuid().ToString()));
        //}

        [Test]
        public async Task GetPromoCodesAsync_ShouldReturnOnlyActivePromoCodes()
        {
            // Arrange
            var activePromoCode = new PromoCode
            {
                Name = "ACTIVEPROMO",
                Discount = 10,
                IsActive = true
            };
            var inactivePromoCode = new PromoCode
            {
                Name = "INACTIVEPROMO",
                Discount = 20,
                IsActive = false
            };
            dbContext.PromoCodes.Add(activePromoCode);
            dbContext.PromoCodes.Add(inactivePromoCode);
            await dbContext.SaveChangesAsync();

            // Act
            var promoCodes = await promoCodeService.GetPromoCodesAsync();

            // Assert
            Assert.That(promoCodes.Count, Is.EqualTo(1));
            Assert.IsTrue(promoCodes.All(pc => pc.IsActive));
        }

    }
}