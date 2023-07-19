using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservationSystem.Data.Models;

public class EquipmentReservations
{
    [ForeignKey(nameof(Equipment))]
    public int EquipmentId { get; set; }

    [Required]
    public Equipment Equipment { get; set; } = null!;

    [ForeignKey(nameof(Reservation))]

    public Guid ReservationId { get; set; }

    [Required]
    public Reservation Reservation { get; set; } = null!;
}