using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReservationSystem.Data.Models;

namespace ReservationSystem.Data.Configurations;

public class ReservationEntityConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder
            .HasOne(r => r.Location)
            .WithMany(l => l.Reservations)
            .HasForeignKey(r => r.LocationId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(r => r.User)
            .WithMany(u => u.UserReservations)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
