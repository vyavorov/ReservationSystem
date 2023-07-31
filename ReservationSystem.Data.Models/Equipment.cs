using System.ComponentModel.DataAnnotations;
using static ReservationSystem.Common.EntityValidationConstants.Equipment;

namespace ReservationSystem.Data.Models;

public class Equipment
{
    public Equipment()
    {
        this.EquipmentReservations = new HashSet<EquipmentReservations>();
    }

    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(EquipmentNameMaxLength)]
    public string Name { get; set; } = null!;

    public ICollection<EquipmentReservations>? EquipmentReservations { get; set; }
}