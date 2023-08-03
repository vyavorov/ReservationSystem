using System.ComponentModel.DataAnnotations;
using static ReservationSystem.Common.EntityValidationConstants.PromoCode;

namespace ReservationSystem.Data.Models;

public class PromoCode
{
    public PromoCode()
    {
        this.Id = Guid.NewGuid();
    }

    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(PromoCodeMaxLength)]
    public string Name { get; set; } = null!;

    [Required]
    public int Discount { get; set; }

    public bool IsActive { get; set; } = true;
}