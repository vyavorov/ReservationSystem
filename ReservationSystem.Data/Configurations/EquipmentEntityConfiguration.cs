using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReservationSystem.Data.Models;

namespace ReservationSystem.Data.Configurations;

public class EquipmentEntityConfiguration : IEntityTypeConfiguration<Equipment>
{
    public void Configure(EntityTypeBuilder<Equipment> builder)
    {
        builder.HasData(this.GenerateEquipments());
    }

    private Equipment[] GenerateEquipments()
    {
        ICollection<Equipment> equipments = new HashSet<Equipment>();
        Equipment equipment;
        equipment = new Equipment()
        {
            Id = 1,
            Name = "Monitor"
        };
        equipments.Add(equipment);
        equipment = new Equipment()
        {
            Id = 2,
            Name = "Keyboard"
        };
        equipments.Add(equipment);
        equipment = new Equipment()
        {
            Id = 3,
            Name = "Mouse"
        };
        equipments.Add(equipment);
        return equipments.ToArray();
    }
}
