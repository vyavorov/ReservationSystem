using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using ReservationSystem.Data;
using ReservationSystem.Data.Models;
using ReservationSystem.Services;
using ReservationSystem.Services.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationSystem.Services.Tests
{
    public class EquipmentServiceTests
    {
        private ReservationDbContext dbContext;
        private IEquipmentService equipmentService;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ReservationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_" + System.Guid.NewGuid().ToString())
                .Options;

            dbContext = new ReservationDbContext(options);
            equipmentService = new EquipmentService(dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Database.EnsureDeleted();
        }

        [Test]
        public async Task CreateEquipmentAsync_ShouldAddEquipmentToDatabase()
        {
            // Arrange
            var equipment = new Equipment
            {
                Name = "Test Equipment"
            };

            // Act
            await equipmentService.CreateEquipmentAsync(equipment);

            // Assert
            Assert.That(dbContext.Equipments.Count(),Is.EqualTo(1));
            Assert.That(dbContext.Equipments.First().Name, Is.EqualTo("Test Equipment"));
        }

        [Test]
        public async Task GetEquipmentByIdAsync_ShouldReturnCorrectEquipment()
        {
            // Arrange
            var equipment = new Equipment
            {
                Name = "Test Equipment"
            };
            dbContext.Equipments.Add(equipment);
            await dbContext.SaveChangesAsync();

            // Act
            var result = await equipmentService.GetEquipmentByIdAsync(equipment.Id);

            // Assert
            Assert.That(result.Name, Is.EqualTo(equipment.Name));
        }

        [Test]
        public async Task GetAllEquipmentsAsync_ShouldReturnAllActiveEquipments()
        {
            // Arrange
            var equipment1 = new Equipment { Name = "Equipment1" };
            var equipment2 = new Equipment { Name = "Equipment2", IsActive = false }; // inactive equipment
            var equipment3 = new Equipment { Name = "Equipment3" };

            dbContext.Equipments.AddRange(equipment1, equipment2, equipment3);
            await dbContext.SaveChangesAsync();

            // Act
            var result = await equipmentService.GetAllEquipmentsAsync();

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));  // It should only return 2 active equipments
        }

        [Test]
        public async Task DeleteEquipmentAsync_ShouldMarkEquipmentAsInactive()
        {
            // Arrange
            var equipment = new Equipment
            {
                Name = "Test Equipment"
            };
            dbContext.Equipments.Add(equipment);
            await dbContext.SaveChangesAsync();

            // Act
            await equipmentService.DeleteEquipmentAsync(equipment.Id, equipment);
            var updatedEquipment = await dbContext.Equipments.FindAsync(equipment.Id);

            // Assert
            Assert.IsFalse(updatedEquipment.IsActive);
        }
    }
}