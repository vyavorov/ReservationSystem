using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReservationSystem.Data.Models;

namespace ReservationSystem.Data.Configurations;

public class EquipmentReservationEntityConfiguration : IEntityTypeConfiguration<EquipmentReservations>
{
    public void Configure(EntityTypeBuilder<EquipmentReservations> builder)
    {
        builder.HasKey(eq => new { eq.EquipmentId, eq.ReservationId });
    }
}
