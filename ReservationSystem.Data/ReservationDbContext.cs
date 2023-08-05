using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReservationSystem.Data.Models;
using System.Reflection;

namespace ReservationSystem.Data
{
    public class ReservationDbContext : IdentityDbContext<ApplicationUser,IdentityRole<Guid>,Guid>
    {
        public ReservationDbContext(DbContextOptions<ReservationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Reservation> Reservations { get; set; } = null!;

        public DbSet<Equipment> Equipments { get; set; } = null!;

        public DbSet<EquipmentReservations> EquipmentsReservations { get; set; } = null!;

        public DbSet<Location> Locations { get; set; } = null!;

        public DbSet<PromoCode> PromoCodes { get; set; } = null!;

        public DbSet<Review> Reviews { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            Assembly configAssembly = Assembly.GetAssembly(typeof(ReservationDbContext)) ?? Assembly.GetExecutingAssembly();
            builder.ApplyConfigurationsFromAssembly(configAssembly);

            base.OnModelCreating(builder);
        }
    }
}