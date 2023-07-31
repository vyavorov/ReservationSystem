using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static ReservationSystem.Common.EntityValidationConstants;

namespace ReservationSystem.Data.Models;

public class Reservation
{
    public Reservation()
    {
        this.Id = Guid.NewGuid();
        this.EquipmentNeeded = new HashSet<EquipmentReservations>();
    }

    [Key]
    public Guid Id { get; set; }

    [Required]
    public DateTime From { get; set; }

    [Required]
    public DateTime To { get; set; }

    [Required]
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    [Required]
    public int CustomersCount { get; set; }

    public string? AdditionalInformation { get; set; }

    public ICollection<EquipmentReservations> EquipmentNeeded { get; set; }

    [Required]
    public decimal TotalPrice { get; set; }

    public int? Discount { get; set; }

    [ForeignKey(nameof(PromoCode))]
    public Guid? PromoCodeId { get; set; }

    public PromoCode? PromoCode { get; set; }

    [Required]
    [ForeignKey(nameof(Location))]
    public int LocationId { get; set; }

    public Location Location { get; set; } = null!;

    [Required]
    [ForeignKey(nameof(UserId))]
    public ApplicationUser User { get; set; } = null!;

    [Required]
    public Guid UserId { get; set; }

    [Required]
    public string PhoneNumber { get; set; } = null!;
}