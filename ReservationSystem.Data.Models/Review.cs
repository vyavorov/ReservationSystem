using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservationSystem.Data.Models;

public class Review
{
    [Required]
    [Key]
    public int Id { get; set; }

    [Required]
    [ForeignKey(nameof(User))]
    public Guid UserId { get; set; }

    [Required]
    public ApplicationUser User { get; set; } = null!;

    [Required]
    [ForeignKey(nameof(LocationId))]
    public Location Location { get; set; } = null!;

    public int LocationId { get; set; }

    [Required]
    public string Comment { get; set; } = null!;
}
